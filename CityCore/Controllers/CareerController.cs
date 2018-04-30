using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CityCore.Models;
using Microsoft.AspNetCore.Identity;
using CityCore.Services;
using Microsoft.Extensions.Logging;
using CityCore.Data;
using Newtonsoft.Json;

namespace CityCore.Controllers
{
    public class CareerController : BaseController
    {
        public CareerController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILogger<AccountController> logger, ApplicationDbContext context) : base(userManager, signInManager, emailSender, logger, context)
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ContentResult> GetList()
        {
            var data = string.Empty;
            try
            {
                string errorMessage = string.Empty;
                var queryStrings = Request.Query;
                var parameters = GetParameters(new List<string>() { "Id", "ProNo", "Department", "Post", "StartDate", "EndDate", "", "" });

                IQueryable<CareerViewModel> finallist;

                finallist = (from Career in _context.Careers
                             select new CareerViewModel
                             {
                                 CreatedOn = Career.CreatedOn,
                                 Department = Career.Department,
                                 FromDocName = _context.Documents.Where(d => d.Id == Career.FormDocumentId).Select(m => m.FileName).FirstOrDefault(),
                                 PostDocName = _context.Documents.Where(d => d.Id == Career.DocumentId).Select(m => m.FileName).FirstOrDefault(),
                                 FormDocURL = _context.Documents.Where(d => d.Id == Career.FormDocumentId).Select(m => m.URL).FirstOrDefault(),
                                 PostDocURL = _context.Documents.Where(d => d.Id == Career.DocumentId).Select(m => m.URL).FirstOrDefault(),
                                 StarDate = Career.StarDate,
                                 EndDate = Career.EndDate,
                                 PostName = Career.PostName,
                                 Id = Career.Id,
                                 PRONo = Career.PRONo
                             });
                //}

                #region Filters


                if (parameters.ContainsKey("searchBy") && !string.IsNullOrWhiteSpace(parameters["searchBy"]))
                {
                    var acode = parameters["searchBy"].ToString().ToLower();
                    finallist = finallist.Where(p => p.PRONo.ToLower().Contains(acode) || p.PostName.ToLower().Contains(acode)
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
                            finallist = finallist.OrderByDescending(x => x.PRONo);
                        }
                        else
                        {
                            finallist = finallist.OrderBy(x => x.PRONo);
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
                            finallist = finallist.OrderByDescending(x => x.PostName);
                        }
                        else
                        {
                            finallist = finallist.OrderBy(x => x.PostName);
                        }
                        break;


                    case "StartDate":
                        if (SortDir == "desc")
                        {
                            finallist = finallist.OrderByDescending(x => x.StarDate);
                        }
                        else
                        {
                            finallist = finallist.OrderBy(x => x.StarDate);
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
                                         model.PRONo,
                                         model.Department,
                                         model.PostName,
                                         model.StarDate.ToString("dd/MM/yyyy"),
                                         model.EndDate.ToString("dd/MM/yyyy"),
                                         model.PostDocName,
                                         model.FromDocName,
                                         model.PostDocURL,
                                         model.FormDocURL

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
    }
}