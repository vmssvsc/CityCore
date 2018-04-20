using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using admincore.Models;
using admincore.Services;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using admincore.Models.Gallery;
using System.Linq;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using admincore.Data;
using Microsoft.Extensions.Options;
using admincore.Common;
using admincore.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace admincore.Controllers
{
    [Authorize]
    public class NewsController : BaseController
    {
        public NewsController(UserManager<ApplicationUser> userManager,
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
                var parameters = GetParameters(new List<string>() { "Id", "Title", "Date", "Priority", "NewsType", "Status", "NoOfImages", "Action" });

                IQueryable<NewsListViewModel> finallist;

                finallist = (from news in _context.News
                             select new NewsListViewModel
                             {
                               CreatedOn = news.CreatedOn,
                               Date = news.Date,
                               Id = news.Id,
                               NoOfImages = _context.NewsDocumentMaps.Where(s => s.NewsId == news.Id).Count(),
                               Priority = news.Priority,
                               NewsType = news.NewsType,
                               Title = news.Title,
                               Status = news.Status
                             });
                //}

                #region Filters


                //if (parameters.ContainsKey("searchBy") && !string.IsNullOrWhiteSpace(parameters["searchBy"]))
                //{
                //    var acode = parameters["searchBy"].ToString();
                //    finallist = finallist.Where(p => p.Title.ToLower().Contains(acode) || p.Description.ToLower().Contains(acode)
                //    );
                //}

                //if (!string.IsNullOrWhiteSpace(queryStrings["date"]) && queryStrings["date"].ToString() != "")
                //{
                //    var l = System.Net.WebUtility.UrlDecode(queryStrings["date"]);
                //    finallist = finallist.Where(p => p.CreatedOn == Convert.ToDateTime(l));
                //}

                #endregion

                int TotalCount = finallist.Count();

                #region Sorting

                string SortColumn = parameters["sort_by"].ToString();
                string SortDir = parameters["sort_order"].ToString();
                // "Id", "Title", "Date", "Priority", "Status", "NoOfImages", "Action" 
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
                    case "NewsType":
                        if (SortDir == "desc")
                        {
                            finallist = finallist.OrderByDescending(x => x.NewsType);
                        }
                        else
                        {
                            finallist = finallist.OrderBy(x => x.NewsType);
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
                    case "NoOfImages":
                        if (SortDir == "desc")
                        {
                            finallist = finallist.OrderByDescending(x => x.NoOfImages);
                        }
                        else
                        {
                            finallist = finallist.OrderBy(x => x.NoOfImages);
                        }
                        break;
                    default:
                        finallist = finallist.OrderBy(x => x.CreatedOn);
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
                                         model.Priority.ToString(),
                                         model.NewsType.ToString(),
                                         model.Status.ToString(),
                                         model.NoOfImages.ToString()

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

        public async Task<IActionResult> AddEdit(int id)
        {
            await SetUserData();
            var model = new NewsViewModel()
            {
                Date = DateTime.UtcNow
            };
            if (id > 0)
            {
                var rec = _context.News.Where(a => a.Id == id).Select(j => new NewsViewModel
                {
                    Id = j.Id,
                    Title = j.Title,
                    Date = j.Date,
                    Description = j.Description,
                    Priority = j.Priority,
                    Status = j.Status,
                    NewsType = j.NewsType
                }).FirstOrDefault();

                if (rec != null)
                    model = rec;
            }

            var statusList = from Enums.NewsStatus es in Enum.GetValues(typeof(Enums.NewsStatus))
                             select new { Id = es, Name = es.ToString() };

            var priorityList = from Enums.NewsPriority es in Enum.GetValues(typeof(Enums.NewsPriority))
                               select new { Id = es, Name = es.ToString() };

            var typeList = from Enums.NewsType es in Enum.GetValues(typeof(Enums.NewsType))
                               select new { Id = es, Name = es.ToString() };

            ViewBag.Statuses = new SelectList(statusList, "Id", "Name");

            ViewBag.Priorities = new SelectList(priorityList, "Id", "Name");

            ViewBag.NewsTypes = new SelectList(typeList, "Id", "Name");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(NewsViewModel model)
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

                        if (model.Id > 0)
                        {
                            var rec = _context.News.Where(e => e.Id == model.Id).FirstOrDefault();
                            if (rec == null)
                                throw new Exception("Record not found.");

                            rec.ModifiedBy = user.Id;
                            rec.ModifiedOn = DateTime.UtcNow;

                            rec.Date = model.Date;
                            rec.Description = model.Description;
                            rec.Priority = model.Priority;
                            rec.Status = model.Status;
                            rec.Title = model.Title;
                            rec.NewsType = model.NewsType;
                            _context.Update(rec);

                            _context.SaveChanges();
                            transaction.Commit();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            var newsRec = new News
                            {
                                CreatedBy = user.Id,
                                CreatedOn = DateTime.UtcNow,
                                Date = model.Date,
                                Description = model.Description,
                                Priority = model.Priority,
                                Status = model.Status,
                                Title = model.Title,
                                NewsType = model.NewsType
                            };
                            _context.News.Add(newsRec);
                            _context.SaveChanges();

                            if (model.Images != null && model.Images.Count > 0 && !model.Images.Where(f => f.Length == 0).Any())
                            {
                                foreach (var item in model.Images)
                                {
                                    //Upload files
                                    var fileRes = await _documentManager.Save(item, _amazonSettings.SliderBucketName);

                                    fileRes.DocumentCategory = Enums.DocumentCategory.NewsImage;
                                    fileRes.CreatedBy = user.Id;

                                    _context.Add(new NewsDocumentMap()
                                    {
                                        CreatedBy = user.Id,
                                        CreatedOn = DateTime.UtcNow,
                                        DocumentId = fileRes.Id,
                                        NewsId = newsRec.Id,
                                    });
                                }

                                _context.SaveChanges();
                                transaction.Commit();
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Please add atleast one photo.");
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
            var statusList = from Enums.NewsStatus es in Enum.GetValues(typeof(Enums.NewsStatus))
                             select new { Id = es, Name = es.ToString() };

            var priorityList = from Enums.NewsPriority es in Enum.GetValues(typeof(Enums.NewsPriority))
                               select new { Id = es, Name = es.ToString() };

            var typeList = from Enums.NewsType es in Enum.GetValues(typeof(Enums.NewsType))
                           select new { Id = es, Name = es.ToString() };

            ViewBag.Statuses = new SelectList(statusList, "Id", "Name");

            ViewBag.Priorities = new SelectList(priorityList, "Id", "Name");

            ViewBag.NewsTypes = new SelectList(typeList, "Id", "Name");
            return View("AddEdit", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var rec = _context.News.Where(s => s.Id == id).FirstOrDefault();
                    if (rec == null)
                        throw new Exception("Invalid News id.");

                    var photos = _context.NewsDocumentMaps.Where(a => a.NewsId == rec.Id).ToList();
                    if (photos.Count > 0)
                    {
                        foreach (var item in photos)
                        {
                            var res = await _documentManager.Delete(item.DocumentId);
                        }
                        var removedPics = _context.NewsDocumentMaps.Where(m => m.NewsId == rec.Id).ToList();
                        _context.RemoveRange(removedPics);
                    }
                    _context.Remove(rec);
                    _context.SaveChanges();
                    transaction.Commit();
                    return Json(new { success = true, message = "News deleted successfully." });

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