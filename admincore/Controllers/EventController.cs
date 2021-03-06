﻿using Microsoft.AspNetCore.Identity;
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


                //if (parameters.ContainsKey("searchBy") && !string.IsNullOrWhiteSpace(parameters["searchBy"]))
                //{
                //    var acode = parameters["searchBy"].ToString();
                //    finallist = finallist.Where(p => p.Title.ToLower().Contains(acode) || p.Description.ToLower().Contains(acode)
                //    );
                //}

                if (!string.IsNullOrWhiteSpace(queryStrings["date"]) && queryStrings["date"].ToString() != "")
                {
                    var l = System.Net.WebUtility.UrlDecode(queryStrings["date"]);
                    finallist = finallist.Where(p => p.Date == Convert.ToDateTime(l));
                }

                #endregion

                int TotalCount = finallist.Count();

                #region Sorting

                string SortColumn = parameters["sort_by"].ToString();
                string SortDir = "";//parameters["sort_order"].ToString();
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
                    default:
                        finallist = finallist.OrderBy(x => x.Status);
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
            var model = new EventViewModel()
            {
                Date = DateTime.UtcNow
            };

            if (Id > 0)
            {
                var rec = _context.Events.Where(e => e.Id == Id).Select(k => new EventViewModel()
                {
                    Id = k.Id,
                    Date = k.EventDate,
                    Description = k.Description,
                    Title = k.Title,
                    DocId = k.DocumentId ?? 0,
                    ImgId = k.ImageDocumentId ?? 0,
                    DocName = _context.Documents.Where(d => d.Id == k.DocumentId).Select(m => m.FileName).FirstOrDefault(),
                    ImageName = _context.Documents.Where(d => d.Id == k.ImageDocumentId).Select(m => m.FileName).FirstOrDefault(),
                    DocUrl = _context.Documents.Where(d => d.Id == k.DocumentId).Select(m => m.URL).FirstOrDefault(),
                    ImageUrl = _context.Documents.Where(d => d.Id == k.ImageDocumentId).Select(m => m.URL).FirstOrDefault(),
                    Priority = k.Priority,
                    EventStatus = k.Status,

                }).FirstOrDefault();

                if (model != null)
                    model = rec;
            }

            await SetUserData();


            var statusList = from Enums.EventStatus es in Enum.GetValues(typeof(Enums.EventStatus))
                             select new { Id = es, Name = es.ToString() };

            var priorityList = from Enums.EventPriority es in Enum.GetValues(typeof(Enums.EventPriority))
                               select new { Id = es, Name = es.ToString() };

            ViewBag.Statuses = new SelectList(statusList, "Id", "Name");

            ViewBag.Priorities = new SelectList(priorityList, "Id", "Name");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(EventViewModel model)
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

                        if (model.Id > 0)
                        {
                            var rec = _context.Events.Where(e => e.Id == model.Id).FirstOrDefault();
                            if (rec == null)
                            {
                                throw new Exception("Record not found.");
                            }
                            Document fileRes = null;
                            Document imageRes = null;
                            //Upload files
                            if (model.File != null)
                            {
                                fileRes = await _documentManager.Save(model.File, _amazonSettings.SliderBucketName);
                                if (fileRes != null)
                                {
                                    fileRes.DocumentCategory = Enums.DocumentCategory.EventFile;
                                    fileRes.CreatedBy = user.Id;
                                    rec.DocumentId = fileRes != null ? fileRes.Id : 0;
                                    if (rec.DocumentId == 0)
                                        rec.DocumentId = null;
                                }
                            }
                            if (model.Image != null)
                            {
                                imageRes = await _documentManager.Save(model.Image, _amazonSettings.SliderBucketName);
                                if (imageRes != null)
                                {
                                    imageRes.DocumentCategory = Enums.DocumentCategory.EventImage;
                                    imageRes.CreatedBy = user.Id;
                                    rec.ImageDocumentId = imageRes != null ? imageRes.Id : 0;
                                    if (rec.ImageDocumentId == 0)
                                        rec.ImageDocumentId = null;
                                }
                            }

                            rec.ModifiedBy = user.Id;
                            rec.ModifiedOn = DateTime.UtcNow;
                           
                            rec.Description = model.Description;
                            rec.EventDate = model.Date;
                            rec.Title = model.Title;
                            rec.Priority = model.Priority;
                            rec.Status = model.EventStatus;
                            _context.Update(rec);

                            _context.SaveChanges();
                            transaction.Commit();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            if (model.File != null && model.Image != null && model.File.Length > 0)
                            {
                                //Upload files
                                var fileRes = await _documentManager.Save(model.File, _amazonSettings.SliderBucketName);
                                var imageRes = await _documentManager.Save(model.Image, _amazonSettings.SliderBucketName);

                                if (fileRes != null && imageRes != null)
                                {
                                    fileRes.DocumentCategory = Enums.DocumentCategory.EventFile;
                                    fileRes.CreatedBy = user.Id;
                                    imageRes.DocumentCategory = Enums.DocumentCategory.EventImage;
                                    imageRes.CreatedBy = user.Id;

                                    _context.Add(new Event()
                                    {
                                        CreatedBy = user.Id,
                                        CreatedOn = DateTime.UtcNow,
                                        DocumentId = fileRes.Id,
                                        Description = model.Description,
                                        EventDate = model.Date,
                                        Title = model.Title,
                                        Priority = model.Priority,
                                        Status = model.EventStatus,
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

            var statusList = from Enums.EventStatus es in Enum.GetValues(typeof(Enums.EventStatus))
                             select new { Id = es, Name = es.ToString() };

            var priorityList = from Enums.EventPriority es in Enum.GetValues(typeof(Enums.EventPriority))
                               select new { Id = es, Name = es.ToString() };

            ViewBag.Statuses = new SelectList(statusList, "Id", "Name");

            ViewBag.Priorities = new SelectList(priorityList, "Id", "Name");

            return View("AddEdit", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var rec = _context.Events.Where(s => s.Id == id).FirstOrDefault();
                    if (rec == null)
                        throw new Exception("Invalid Event id.");

                    var res = rec.DocumentId > 0 ? await _documentManager.Delete(rec.DocumentId ?? 0) : true;

                    var resimage = rec.ImageDocumentId > 0 ? await _documentManager.Delete(rec.ImageDocumentId ?? 0) : true;

                    if (res && resimage)
                    {
                        _context.Remove(rec);
                        _context.SaveChanges();
                        transaction.Commit();
                        return Json(new { success = true, message = "Event deleted successfully." });
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
                    if (type == 1)
                    {
                        var rec = _context.Events.Where(s => s.DocumentId == id).FirstOrDefault();
                        if (rec == null)
                            throw new Exception("Invalid Document.");

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
                        var rec = _context.Events.Where(s => s.ImageDocumentId == id).FirstOrDefault();
                        if (rec == null)
                            throw new Exception("Invalid Image.");

                        var res = rec.ImageDocumentId > 0 ? await _documentManager.Delete(rec.ImageDocumentId ?? 0) : true;
                        if (res)
                        {
                            rec.ImageDocumentId = null;
                            rec.ModifiedBy = user.Id;
                            rec.ModifiedOn = DateTime.UtcNow;
                        }

                        _context.SaveChanges();
                        transaction.Commit();
                        return Json(new { success = true, message = "Image deleted successfully." });
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