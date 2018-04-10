using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using admincore.Models.Project;
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
    public class ABDProjectController : BaseController
    {
        public ABDProjectController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILogger<AccountController> logger, IDocumentManager documentManager, ApplicationDbContext context, IOptions<AmazonSettings> amazonSettings) : base(userManager, signInManager, emailSender, logger, documentManager, context, amazonSettings)
        {
        }




        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ABDProjects()
        {
            return View();
        }
        public async Task<IActionResult> ABDinitiatives()
        {
            await SetUserData();
            return View();
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
                            var rec = _context.ProjectInitiatives.Where(e => e.Id == model.Id).FirstOrDefault();
                            if (rec == null)
                            {
                                throw new Exception("Record not found.");
                            }

                            
                            rec.ModifiedBy = user.Id;
                            rec.ModifiedOn = DateTime.UtcNow;

                            rec.Title = model.Title;
                            rec.Initiative = model.Initiative;

                            

                            
                            _context.Update(rec);

                            _context.SaveChanges();
                            transaction.Commit();
                            return RedirectToAction("ABDinitiatives");
                        }
                        else
                        {
                            var initiative = new ProjectInitiative()
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
                            return RedirectToAction("ABDinitiatives");
                        }

                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", e.Message);
                        return View("InitiativesAddEdit", model);
                        // return Json(new { success = false, message = e.Message });
                    }
                }
            }
            return View("InitiativesAddEdit", model);
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

                IQueryable<ProjectInititativeListModel> finallist;

                finallist = (from ProjectInitiatives in _context.ProjectInitiatives
                             select new ProjectInititativeListModel
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

        public async Task<IActionResult> InitiativesAddEdit(int Id)
        {
            var model = new ProjectInititativeViewModel()
            {

            };

            if (Id > 0)
            {
                var rec = _context.ProjectInitiatives.Where(e => e.Id == Id).Select(k => new ProjectInititativeViewModel()
                {
                    Id = k.Id,

                    Initiative = k.Initiative,
                    Title = k.Title,
                    
                }).FirstOrDefault();

                if (model != null)
                    model = rec;
            }

            await SetUserData();


            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteI(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var rec = _context.ProjectInitiatives.Where(v => v.Id == id).FirstOrDefault();
                    if (rec == null)
                        throw new Exception("Invalid Project Initiative id.");



                    _context.Remove(rec);
                    _context.SaveChanges();
                    transaction.Commit();
                    return Json(new { success = true, message = "Project Initiative deleted successfully." });


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