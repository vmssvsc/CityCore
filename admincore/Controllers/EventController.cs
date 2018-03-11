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

namespace admincore.Controllers
{
    [Authorize]
    public class EventController : BaseController
    {
        public EventController(UserManager<ApplicationUser> userManager,
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
                var parameters = GetParameters(new List<string>() { "Id", "Title", "Date", "Description", "Priority", "Status", "Action" });

                IQueryable<EventListModel> finallist;
 
                finallist = (from Events in _context.Events
                             where Events.Status != Enums.EventStatus.Completed
                             && Events.Status != Enums.EventStatus.Cancelled
                             orderby Events.EventDate 
                             orderby Events.Priority
                             select new EventListModel
                             {
                                 Id = Events.Id,
                                 Date = Events.EventDate,
                                 Description = Events.Description,
                                 Priority = Events.Priority.ToString(),
                                 Status = Events.Status.ToString(),
                                 Title = Events.Title
                             });
                //}

                #region Filters


                if (parameters.ContainsKey("searchBy") && !string.IsNullOrWhiteSpace(parameters["searchBy"]))
                {
                    var acode = parameters["searchBy"].ToString();
                    finallist = finallist.Where(p => p.Title.ToLower().Contains(acode) || p.Description.ToLower().Contains(acode)
                    );
                }

                if (!string.IsNullOrWhiteSpace(queryStrings["date"]) && queryStrings["date"].ToString() != "")
                {
                    var l = System.Net.WebUtility.UrlDecode(queryStrings["date"]);
                    finallist = finallist.Where(p => p.Date == Convert.ToDateTime(l));
                }
              
                #endregion

                int TotalCount = finallist.Count();

                #region Sorting

                string SortColumn = parameters["sort_by"].ToString();
                string SortDir = parameters["sort_order"].ToString();
                //"Id", "Title", "Date", "Description", "Priority", "Status", "Action"
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
                    case "Date":
                        if (SortDir == "desc")
                        {
                            finallist = finallist.OrderByDescending(x => x.Date);
                        }
                        else
                        {
                            finallist = finallist.OrderBy(x => x.Date);
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


                    case "Priority":
                        if (SortDir == "desc")
                        {
                            finallist = finallist.OrderByDescending(x => x.Priority);
                        }
                        else
                        {
                            finallist = finallist.OrderBy(x => x.Priority);
                        }
                        break;
                    case "Status":
                        if (SortDir == "desc")
                        {
                            finallist = finallist.OrderByDescending(x => x.Status);
                        }
                        else
                        {
                            finallist = finallist.OrderBy(x => x.Status);
                        }
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
                                         model.Date.ToString("dd/MM/yyyy"),
                                         model.Description.ToString(),
                                         model.Priority.ToString(),
                                         model.Status.ToString()

                                 };
                   // data = Serialization.Json.Serialize(new { iTotalRecords = TotalCount, iTotalDisplayRecords = TotalCount, aaData = res });
                }
                else
                {
                    //data = Serialization.Json.Serialize(new { iTotalRecords = 0, iTotalDisplayRecords = 0, aaData = new string[] { } });
                }
            }
            catch (Exception ex)
            {
                //data = Serialization.Json.Serialize(new
                //{
                //    iTotalRecords = 0,
                //    iTotalDisplayRecords = 0,
                //    aaData = new string[] { }
                //});
            }

            return Content(data, "application/json");
        }

        public async Task<IActionResult> AddEdit(int Id)
        {
            var model = new EventViewModel();

            if(Id > 0)
            {
                var rec = _context.Events.Where(e => e.Id == Id).Select(k => new EventViewModel()
                {
                    Id = k.Id
                }).FirstOrDefault();

                if (model != null)
                    model = rec;
            }
          
            await SetUserData();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(EventViewModel model)
        {
            await SetUserData();
            return View();
        }
    }
}