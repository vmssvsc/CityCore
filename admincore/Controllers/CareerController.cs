using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using admincore.Models;
using admincore.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using admincore.Data;
using Microsoft.Extensions.Options;
using admincore.Common;
using System.Linq;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using admincore.Data.Models;

namespace admincore.Controllers
{
    [Authorize]
    public class CareerController : BaseController
    {
        public CareerController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILogger<AccountController> logger, IDocumentManager documentManager, ApplicationDbContext context, IOptions<AmazonSettings> amazonSettings) : base(userManager, signInManager, emailSender, logger, documentManager, context, amazonSettings)
        {
        }

        public async Task<IActionResult> Index()
        {
            await SetUserData();
            return View();
        }

       
        public async Task<ContentResult> GetList()
        {
            var data = string.Empty;
            try
            {
                string errorMessage = string.Empty;
                var queryStrings = Request.Query;
                var user = await _userManager.GetUserAsync(User);
                var parameters = GetParameters(new List<string>() { "Id", "ProNo", "Department", "Post", "StartDate", "EndDate", "Action" });

                IQueryable<CareerListViewModel> finallist;

                finallist = (from Career in _context.Careers
                             select new CareerListViewModel
                             {
                                 Department = Career.Department,
                                 EndDate = Career.EndDate.ToString("dd/MM/yyyy"),
                                 Id = Career.Id,
                                 Post = Career.PostName,
                                 ProNo = Career.PRONo,
                                 StartDate = Career.StarDate.ToString("dd/MM/yyyy"),
                                 CreatedOn = Career.CreatedOn
                             });
                //}

                #region Filters


                if (parameters.ContainsKey("searchBy") && !string.IsNullOrWhiteSpace(parameters["searchBy"]))
                {
                    var acode = parameters["searchBy"].ToString().ToLower();
                    finallist = finallist.Where(p => p.ProNo.ToLower().Contains(acode) || p.Post.ToLower().Contains(acode)
                    );
                }

                //if (!string.IsNullOrWhiteSpace(queryStrings["date"]) && queryStrings["date"].ToString() != "")
                //{
                //    var l = System.Net.WebUtility.UrlDecode(queryStrings["date"]);
                //    finallist = finallist.Where(p => p.Date == Convert.ToDateTime(l));
                //}

                #endregion

                int TotalCount = finallist.Count();

                #region Sorting

                string SortColumn = parameters["sort_by"].ToString();
                string SortDir = parameters["sort_order"].ToString();
                //"Id", "ProNo", "Department", "Post", "StartDate", "EndDate", "Action"
                switch (SortColumn)
                {
                    case "ProNo":
                        if (SortDir == "desc")
                        {
                            finallist = finallist.OrderByDescending(x => x.ProNo);
                        }
                        else
                        {
                            finallist = finallist.OrderBy(x => x.ProNo);
                        }
                        break;
                    case "Department":
                        if (SortDir == "desc")
                        {
                            finallist = finallist.OrderByDescending(x => x.Department);
                        }
                        else
                        {
                            finallist = finallist.OrderBy(x => x.Department);
                        }
                        break;
                    case "Post":
                        if (SortDir == "desc")
                        {
                            finallist = finallist.OrderByDescending(x => x.Post);
                        }
                        else
                        {
                            finallist = finallist.OrderBy(x => x.Post);
                        }
                        break;


                    case "StartDate":
                        if (SortDir == "desc")
                        {
                            finallist = finallist.OrderByDescending(x => x.StartDate);
                        }
                        else
                        {
                            finallist = finallist.OrderBy(x => x.StartDate);
                        }
                        break;
                    case "EndDate":
                        if (SortDir == "desc")
                        {
                            finallist = finallist.OrderByDescending(x => x.EndDate);
                        }
                        else
                        {
                            finallist = finallist.OrderBy(x => x.EndDate);
                        }
                        break;
                    default:
                        finallist = finallist.OrderByDescending(x => x.CreatedOn);
                        break;
                }

                #endregion

                #region Pagination and OutPut


                var pageNo = Convert.ToInt32(parameters["page_number"]);
                var pageSize = Convert.ToInt32(parameters["results_per_page"]);
                var list = finallist.Skip((pageNo - 1) * (pageSize)).Take(pageSize).ToList();

                #endregion

                if (list != null && list.Count > 0)
                {

                    var entityLst = list;

                    foreach (var item in entityLst)
                    {

                    }
                    var res = from model in entityLst
                              select new string[]
                                 {
                                         model.Id.ToString(),
                                         model.ProNo,
                                         model.Department,
                                         model.Post.ToString(),
                                         model.StartDate,
                                         model.EndDate,

                                 };
                    data = JsonConvert.SerializeObject(new { iTotalRecords = TotalCount, iTotalDisplayRecords = TotalCount, aaData = res });
                }
                else
                {
                    data = JsonConvert.SerializeObject(new { iTotalRecords = 0, iTotalDisplayRecords = 0, aaData = new string[] { } });
                }
            }
            catch (Exception ex)
            {
                data = JsonConvert.SerializeObject(new
                {
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = new string[] { }
                });
            }

            return Content(data, "application/json");
        }

        public async Task<IActionResult> AddEdit(int Id)
        {
            await SetUserData();

            var model = new CareerViewModel()
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
            };

            if (Id > 0)
            {
                var rec = _context.Careers.Where(e => e.Id == Id).Select(k => new CareerViewModel()
                {
                    Id = k.Id,
                    EndDate = k.EndDate,                  
                    Department = k.Department,
                    FormDocName = _context.Documents.Where(d => d.Id == k.FormDocumentId).Select(m => m.FileName).FirstOrDefault(),
                    PostDocName = _context.Documents.Where(d => d.Id == k.DocumentId).Select(m => m.FileName).FirstOrDefault(),
                    PostName = k.PostName,
                    PRONo = k.PRONo,
                    StartDate = k.StarDate,
                    PostDocId = k.DocumentId ?? 0,
                    FormDocId = k.FormDocumentId ?? 0,
                    FormDocURL = _context.Documents.Where(d => d.Id == k.FormDocumentId).Select(m => m.URL).FirstOrDefault(),
                    PostDocURL = _context.Documents.Where(d => d.Id == k.DocumentId).Select(m => m.URL).FirstOrDefault(),
                }).FirstOrDefault();

                if (model != null)
                    model = rec;
            }
           
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(CareerViewModel model)
        {
            //await SetUserData();
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        //upload.  1.)ToDo Check if both file's size is > that defined in settings table.
                        //         2.)Add add all model validations.
                        //         3.)Change buckets.
                        //         4.)Set ddl for priority and status. [Done]
                        //         5.)Handle error message. [Done]
                        //         6.)Delete older files. [Done]
                        //         7.)Manage file for edit case. [Done]


                        if (model.StartDate > model.EndDate)
                            throw new Exception("Start date must be less than end date.");


                        if (model.Id > 0)
                        {
                            var rec = _context.Careers.Where(e => e.Id == model.Id).FirstOrDefault();
                            if (rec == null)
                            {
                                throw new Exception("Record not found.");
                            }
                            Document docRes = null;
                            Document formRes = null;
                            //Upload files
                            if (model.PostDocument != null)
                            {
                                docRes = await _documentManager.Save(model.PostDocument, _amazonSettings.SliderBucketName);
                                if (docRes != null)
                                {
                                    docRes.DocumentCategory = Enums.DocumentCategory.CareerFile;
                                    docRes.CreatedBy = user.Id;
                                    rec.DocumentId = docRes != null ? docRes.Id : 0;
                                    if (rec.DocumentId == 0)
                                        rec.DocumentId = null;
                                }
                            }
                            if (model.FormDocument != null)
                            {
                                formRes = await _documentManager.Save(model.FormDocument, _amazonSettings.SliderBucketName);
                                if (formRes != null)
                                {
                                    formRes.DocumentCategory = Enums.DocumentCategory.CareerForm;
                                    formRes.CreatedBy = user.Id;
                                    rec.FormDocumentId = formRes != null ? formRes.Id : 0;
                                    if (rec.FormDocumentId == 0)
                                        rec.FormDocumentId = null;
                                }
                            }

                            rec.ModifiedBy = user.Id;
                            rec.ModifiedOn = DateTime.UtcNow;

                            rec.Department = model.Department;
                            rec.StarDate = model.StartDate;
                            rec.EndDate = model.EndDate;
                            rec.PostName = model.PostName;
                            rec.PRONo = model.PRONo;
                            _context.Update(rec);

                            _context.SaveChanges();
                            transaction.Commit();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            if (model.FormDocument != null && model.PostDocument != null)
                            {
                                //Upload files
                                var docRes = await _documentManager.Save(model.PostDocument, _amazonSettings.SliderBucketName);
                                var formRes = await _documentManager.Save(model.FormDocument, _amazonSettings.SliderBucketName);

                                if (docRes != null && formRes != null)
                                {
                                    docRes.DocumentCategory = Enums.DocumentCategory.CareerFile;
                                    docRes.CreatedBy = user.Id;
                                    formRes.DocumentCategory = Enums.DocumentCategory.CareerForm;
                                    formRes.CreatedBy = user.Id;

                                    _context.Add(new Career()
                                    {
                                        CreatedBy = user.Id,
                                        CreatedOn = DateTime.UtcNow,
                                        DocumentId = docRes.Id,
                                        EndDate = model.EndDate,
                                        PRONo = model.PRONo,
                                        StarDate = model.StartDate,
                                        PostName = model.PostName,
                                        FormDocumentId = formRes.Id,
                                        Department = model.Department,
                                    });
                                    _context.SaveChanges();
                                    transaction.Commit();
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    transaction.Rollback();
                                    return View("AddEdit", model);
                                    //return Json(new { success = false, message = "Upload failed." });
                                }

                            }
                            else
                            {
                                ModelState.AddModelError("", "Please upload both the files.");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", e.Message);
                        return View("AddEdit", model);
                        // return Json(new { success = false, message = e.Message });
                    }
                }
            }
            return View("AddEdit", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var rec = _context.Careers.Where(s => s.Id == id).FirstOrDefault();
                    if (rec == null)
                        throw new Exception("Invalid Career id.");

                    var res = rec.DocumentId > 0 ? await _documentManager.Delete(rec.DocumentId ?? 0) : true;

                    var resimage = rec.FormDocumentId > 0 ? await _documentManager.Delete(rec.FormDocumentId ?? 0) : true;

                    if (res && resimage)
                    {
                        _context.Remove(rec);
                        _context.SaveChanges();
                        transaction.Commit();
                        return Json(new { success = true, message = "Career record deleted successfully." });
                    }
                    else
                    {
                        transaction.Rollback();
                        return Json(new { success = false, message = "Delete failed." });
                    }

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return Json(new { success = false, message = e.Message });
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteDocument(int id, int type)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    var rec = _context.Careers.Where(s => s.DocumentId == id).FirstOrDefault();
                    if (rec == null)
                        throw new Exception("Invalid Document.");

                    if (type == 1)
                    {                     
                        var res = rec.DocumentId > 0 ? await _documentManager.Delete(rec.DocumentId ?? 0) : true;
                        if (res)
                        {
                            rec.DocumentId = null;
                            rec.ModifiedBy = user.Id;
                            rec.ModifiedOn = DateTime.UtcNow;
                        }

                        _context.SaveChanges();
                        transaction.Commit();
                        return Json(new { success = true, message = "File deleted successfully." });
                    }
                    else
                    {                     
                        var res = rec.FormDocumentId > 0 ? await _documentManager.Delete(rec.FormDocumentId ?? 0) : true;
                        if (res)
                        {
                            rec.FormDocumentId = null;
                            rec.ModifiedBy = user.Id;
                            rec.ModifiedOn = DateTime.UtcNow;
                        }

                        _context.SaveChanges();
                        transaction.Commit();
                        return Json(new { success = true, message = "File deleted successfully." });
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return Json(new { success = false, message = e.Message });
                }
            }
        }
    }
}