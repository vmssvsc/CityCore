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
     public class TeamMemberController : BaseController
    {

        public TeamMemberController(UserManager<ApplicationUser> userManager,
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
        public async Task<IActionResult> Save(TeamMemberViewModel model)
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
                            var rec = _context.TeamMembers.Where(e => e.Id == model.Id).FirstOrDefault();
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
                                    imageRes.DocumentCategory = Enums.DocumentCategory.TeamMemberImage;
                                    imageRes.CreatedBy = user.Id;
                                    rec.ImageDocumentId = imageRes != null ? imageRes.Id : 0;

                                }
                            }

                            rec.ModifiedBy = user.Id;
                            rec.ModifiedOn = DateTime.UtcNow;
                            rec.Name = model.Name;
                            rec.Post = model.Post;
                            _context.Update(rec);
                            _context.SaveChanges();
                            transaction.Commit();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            if (model.Image != null)
                            {
                                //Upload files

                                var imageRes = await _documentManager.Save(model.Image, _amazonSettings.SliderBucketName);

                                if (imageRes != null)
                                {

                                    imageRes.DocumentCategory = Enums.DocumentCategory.TeamMemberImage;
                                    imageRes.CreatedBy = user.Id;

                                    _context.Add(new TeamMember()
                                    {
                                        CreatedBy = user.Id,
                                        CreatedOn = DateTime.UtcNow,
                                        Post = model.Post,
                                        Name = model.Name,
                                        ImageDocumentId = imageRes.Id,
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
                var parameters = GetParameters(new List<string>() { "Id", "Name", "Post", "Action" });

                IQueryable<TeamMemberListModel> finallist;

                finallist = (from TeamMembers in _context.TeamMembers
                             select new TeamMemberListModel
                             {
                                 Id = TeamMembers.Id,
                                 Name = TeamMembers.Name,
                                 Post = TeamMembers.Post
                             });


                int TotalCount = finallist.Count();

                #region Sorting

                string SortColumn = parameters["sort_by"].ToString();
                string SortDir = "";
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
                                         model.Post.ToString(),
                                         
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
            var model = new TeamMemberViewModel()
            {

            };

            if (Id > 0)
            {
                var rec = _context.TeamMembers.Where(e => e.Id == Id).Select(k => new TeamMemberViewModel()
                {
                    Id = k.Id,
                    Post = k.Post,
                    Name = k.Name,
                    ImgId = k.ImageDocumentId ?? 0,
                    ImageName = _context.Documents.Where(d => d.Id == k.ImageDocumentId).Select(m => m.FileName).FirstOrDefault(),
                    ImageUrl = _context.Documents.Where(d => d.Id == k.ImageDocumentId).Select(m => m.URL).FirstOrDefault(),
                   
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
                    var rec = _context.TeamMembers.Where(s => s.Id == id).FirstOrDefault();
                    if (rec == null)
                        throw new Exception("Invalid Team Member id.");

                    var resimage = rec.ImageDocumentId > 0 ? await _documentManager.Delete(rec.ImageDocumentId ?? 0) : true;

                    if (resimage)
                    {
                        var updatedRec = _context.TeamMembers.Where(s => s.Id == id).FirstOrDefault();
                        if (updatedRec != null)
                        {
                            _context.Remove(updatedRec);
                            _context.SaveChanges();
                        }
                        transaction.Commit();
                        return Json(new { success = true, message = "Team Member deleted successfully." });
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

                    var rec = _context.TeamMembers.Where(s => s.ImageDocumentId == id).FirstOrDefault();
                    if (rec == null)
                        throw new Exception("Invalid Image.");

                    var res = rec.ImageDocumentId > 0 ? await _documentManager.Delete(rec.ImageDocumentId ?? 0) : true;
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