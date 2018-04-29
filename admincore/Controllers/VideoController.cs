using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using admincore.Models;
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
    public class VideoController : BaseController
    {


        public VideoController(UserManager<ApplicationUser> userManager,
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
                var parameters = GetParameters(new List<string>() { "Id", "Title", "Description", "Action" });

                IQueryable<VideoListModel> finallist;

                finallist = (from Videos in _context.Videos
                             select new VideoListModel
                             {
                                 Id = Videos.Id,
                                 Description = Videos.Description,
                                 Title = Videos.Title
                             });

                int TotalCount = finallist.Count();


                #region Sorting

                string SortColumn = parameters["sort_by"].ToString();
                string SortDir = "";//parameters["sort_order"].ToString();

                switch (SortColumn)
                {
                    case "Title":
                        if (SortDir == "desc")
                        {
                            finallist = finallist.OrderByDescending(x => x.Title);
                        }
                        else
                        {
                            finallist = finallist.OrderBy(x => x.Title);
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
                        finallist = finallist.OrderBy(x => x.Title);
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
                                         model.Title .ToString(),

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
            var model = new VideoViewModel()
            {
                
            };

            if (Id > 0)
            {
                var rec = _context.Videos.Where(e => e.Id == Id).Select(k => new VideoViewModel()
                {
                    Id = k.Id,

                    Description = k.Description,
                    Title = k.Title,
                    VideoUrl = k.URL
                }).FirstOrDefault();

                if (model != null)
                    model = rec;
            }

            await SetUserData();


            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Save(VideoViewModel model)
        {
            //await SetUserData();
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model.VideoUrl.Contains("watch?v="))
                            model.VideoUrl = model.VideoUrl.Replace("watch?v=", "embed/");

                        if (model.Id > 0)
                        {
                            var rec = _context.Videos.Where(e => e.Id == model.Id).FirstOrDefault();
                            if (rec == null)
                            {
                                throw new Exception("Record not found.");
                            }
                            rec.ModifiedBy = user.Id;
                            rec.ModifiedOn = DateTime.UtcNow;
                            rec.Description = model.Description;
                            rec.Title = model.Title;                          
                            rec.URL = model.VideoUrl;

                            // https://www.youtube.com/watch?v=xpVfcZ0ZcFM
                            // https://www.youtube.com/embed/gcwyiOFTAuw

                            _context.Update(rec);

                            _context.SaveChanges();
                            transaction.Commit();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            var vdo = new Video()
                            {
                                CreatedBy = user.Id,
                                CreatedOn = DateTime.UtcNow,

                                Description = model.Description,

                                Title = model.Title,

                                URL = model.VideoUrl,
                            };  
                            _context.Videos.Add(new Video()
                            {
                                CreatedBy = user.Id,
                                CreatedOn = DateTime.UtcNow,

                                Description = model.Description,

                                Title = model.Title,

                                URL = model.VideoUrl,
                            });
                            _context.SaveChanges();
                            transaction.Commit();
                            return RedirectToAction("Index");
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
                    var rec = _context.Videos.Where(v => v.Id == id).FirstOrDefault();
                    if (rec == null)
                        throw new Exception("Invalid Video id.");



                    _context.Remove(rec);
                    _context.SaveChanges();
                    transaction.Commit();
                    return Json(new { success = true, message = "Video deleted successfully." });


                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(new { success = false, message = "Delete failed." });
                }
            }
        }


    }
}
