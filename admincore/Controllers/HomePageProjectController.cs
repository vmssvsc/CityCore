using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using admincore.Models;
using admincore.Models.SmartProject;
using admincore.Services;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using admincore.Data;
using Microsoft.Extensions.Options;
using admincore.Common;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using admincore.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace admincore.Controllers
{
    [Authorize]
    public class HomePageProjectController :  BaseController
    {

        public HomePageProjectController(UserManager<ApplicationUser> userManager,
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

        [HttpPost]
        public async Task<IActionResult> Save(SmartProjectViewModel model)
        {
            //await SetUserData();
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        

                        if (model.Id > 0)
                        {
                            var rec = _context.SmartCityProjects.Where(e => e.Id == model.Id).FirstOrDefault();
                            if (rec == null)
                            {
                                throw new Exception("Record not found.");
                            }
                            
                            Document imageRes = null;
                            
                            if (model.Image != null)
                            {
                                imageRes = await _documentManager.Save(model.Image, _amazonSettings.SliderBucketName);
                                if (imageRes != null)
                                {
                                    imageRes.DocumentCategory = Enums.DocumentCategory.SmartProjectImage;
                                    imageRes.CreatedBy = user.Id;
                                    rec.DocumentId = imageRes != null ? imageRes.Id : 0;

                                }
                            }

                            rec.ModifiedBy = user.Id;
                            rec.ModifiedOn = DateTime.UtcNow;

                            rec.Description = model.Description;
                            rec.Name = model.Name;
                            _context.Update(rec);

                            _context.SaveChanges();
                            transaction.Commit();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            if ( model.Image != null )
                            {
                                //Upload files
                                
                                var imageRes = await _documentManager.Save(model.Image, _amazonSettings.SliderBucketName);

                                if ( imageRes != null)
                                {
                                    
                                    imageRes.DocumentCategory = Enums.DocumentCategory.SmartProjectImage;
                                    imageRes.CreatedBy = user.Id;

                                    _context.Add(new SmartCityProject()
                                    {
                                        CreatedBy = user.Id,
                                        CreatedOn = DateTime.UtcNow,
                                        
                                        Description = model.Description,
                                        Name = model.Name,

                                        DocumentId = imageRes.Id,
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
                                ModelState.AddModelError("", "Please upload  the Image");
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

        public async Task<ContentResult> GetList()
        {
            var data = string.Empty;
            try
            {
                string errorMessage = string.Empty;
                var queryStrings = Request.Query;
                var user = await _userManager.GetUserAsync(User);
                var parameters = GetParameters(new List<string>() { "Id", "Name","Description", "Action" });

                IQueryable<SmartProjectListModel> finallist;

                finallist = (from SmartCityProjects in _context.SmartCityProjects
                             select new SmartProjectListModel
                             {
                                 Id = SmartCityProjects.Id,
                                 
                                 Description = SmartCityProjects.Description,
                                 
                                 Name = SmartCityProjects.Name
                             });
                

                      int TotalCount = finallist.Count();

                #region Sorting

                string SortColumn = parameters["sort_by"].ToString();
                string SortDir = "";//parameters["sort_order"].ToString();
                //"Id", "Title", "Date", "Description", "Priority", "Status", "Action"
                switch (SortColumn)
                {
                    case "Name":
                        if (SortDir == "desc")
                        {
                            finallist = finallist.OrderByDescending(x => x.Name);
                        }
                        else
                        {
                            finallist = finallist.OrderBy(x => x.Name);
                        }
                        break;
                    
                    case "Description":
                        if (SortDir == "desc")
                        {
                            finallist = finallist.OrderByDescending(x => x.Description);
                        }
                        else
                        {
                            finallist = finallist.OrderBy(x => x.Description);
                        }
                        break;

                    default:
                        finallist = finallist.OrderBy(x => x.Name);
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
                                         model.Name .ToString(),
                                         model.Description.ToString(),
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
            var model = new SmartProjectViewModel()
            {
                
            };

            if (Id > 0)
            {
                var rec = _context.SmartCityProjects.Where(e => e.Id == Id).Select(k => new SmartProjectViewModel()
                {
                    Id = k.Id,
                    
                    Description = k.Description,
                    Name = k.Name,
                    
                    ImgId = k.DocumentId ,
                    
                    ImageName = _context.Documents.Where(d => d.Id == k.DocumentId).Select(m => m.FileName).FirstOrDefault(),

                    Url = _context.Documents.Where(d => d.Id == k.DocumentId).Select(m => m.URL).FirstOrDefault()
                    

                }).FirstOrDefault();

                if (model != null)
                    model = rec;
            }

            await SetUserData();


            
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var rec = _context.SmartCityProjects.Where(s => s.Id == id).FirstOrDefault();
                    if (rec == null)
                        throw new Exception("Invalid Smart Project id.");
                 
                    var resimage = rec.DocumentId > 0 ? await _documentManager.Delete(rec.DocumentId ) : true;

                    if ( resimage)
                    {
                        var updatedRec = _context.SmartCityProjects.Where(s => s.Id == id).FirstOrDefault();
                        if (updatedRec != null)
                        {
                            _context.Remove(updatedRec);
                            _context.SaveChanges();
                        }
                        transaction.Commit();
                        return Json(new { success = true, message = "Smart Project deleted successfully." });
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
        public async Task<IActionResult> DeleteDocument(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    
                    
                    
                        var rec = _context.SmartCityProjects.Where(s => s.DocumentId == id).FirstOrDefault();
                        if (rec == null)
                            throw new Exception("Invalid Image.");

                        var res = rec.DocumentId > 0 ? await _documentManager.Delete(rec.DocumentId) : true;
                        if (res)
                        {
                            
                            rec.ModifiedBy = user.Id;
                            rec.ModifiedOn = DateTime.UtcNow;
                        }

                        _context.SaveChanges();
                        transaction.Commit();
                        return Json(new { success = true, message = "Image deleted successfully." });
                    
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