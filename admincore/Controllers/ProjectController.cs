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
using admincore.Models.Project;

namespace admincore.Controllers
{
    [Authorize]
    public class ProjectController : BaseController
    {
        public ProjectController(UserManager<ApplicationUser> userManager,
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


        public async Task<ContentResult> GetListI()
        {

            var data = string.Empty;
            try
            {
                string errorMessage = string.Empty;
                var queryStrings = Request.Query;
                var user = await _userManager.GetUserAsync(User);
                var parameters = GetParameters(new List<string>() { "Id", "Title", "Initiative", "Action" });

                IQueryable<ProjectInititativeViewModel> finallist;

                finallist = (from ProjectInitiatives in _context.ProjectInitiatives
                             select new ProjectInititativeViewModel
                             {
                                 Id = ProjectInitiatives.Id,
                                 Initiative = ProjectInitiatives.Initiative,
                                 Title = ProjectInitiatives.Title
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
                    case "Initiative":
                        if (SortDir == "desc")
                        {
                            finallist = finallist.OrderByDescending(x => x.Initiative);
                        }
                        else
                        {
                            finallist = finallist.OrderBy(x => x.Initiative);
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

                                         model.Initiative.ToString(),

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






        [HttpPost]
        public async Task<IActionResult> SaveI(ProjectInititativeViewModel model)
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
                            var rec = _context.ProjectInitiatives.Where(p => p.Id == model.Id).FirstOrDefault();
                            if (rec == null)
                            {
                                throw new Exception("Record not found.");
                            }


                            rec.ModifiedBy = user.Id;
                            rec.ModifiedOn = DateTime.UtcNow;

                            rec.Initiative = model.Initiative;

                            rec.Title = model.Title;
                            _context.Update(rec);

                            _context.SaveChanges();
                            transaction.Commit();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            var vdo = new ProjectInitiative()
                            {
                                CreatedBy = user.Id,
                                CreatedOn = DateTime.UtcNow,

                                Initiative = model.Initiative,

                                Title = model.Title,

                               
                            };
                            _context.ProjectInitiatives.Add(new ProjectInitiative()
                            {
                                CreatedBy = user.Id,
                                CreatedOn = DateTime.UtcNow,

                                Initiative = model.Initiative,

                                Title = model.Title,

                               
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
                        return View("ABDInitiative", model);
                        // return Json(new { success = false, message = e.Message });
                    }
                }
            }
            return View("ABDInitiative", model);
        }




        public async Task<IActionResult> ABDInitiative()
        {
            await SetUserData();
            return View();
        }









        public async Task<IActionResult> ABDProject()
        {
            await SetUserData();
            return View();
        }


    }
}
    
