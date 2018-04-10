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

namespace admincore.Controllers
{
    [Authorize]
    public class GalleryController: BaseController
    {
        public GalleryController(UserManager<ApplicationUser> userManager,
         SignInManager<ApplicationUser> signInManager,
         IEmailSender emailSender,
         ILogger<AccountController> logger, IDocumentManager documentManager, ApplicationDbContext context, IOptions<AmazonSettings> amazonSettings) : base(userManager, signInManager, emailSender, logger, documentManager, context, amazonSettings)
        {
        }


        public async Task<IActionResult> Index()
        {
            await SetUserData();
            var model = new  GalleryViewModel();
            return View(model);
        }

        public async Task<ContentResult> GetList()
        {
            var data = string.Empty;
            try
            {
                string errorMessage = string.Empty;
                var queryStrings = Request.Query;
                var user = await _userManager.GetUserAsync(User);
                var parameters = GetParameters(new List<string>() { "Id", "Title", "Date", "NoOfPhotos", "Action" });

                IQueryable<AlbumListViewModel> finallist;

                finallist = (from Album in _context.Albums
                             select new AlbumListViewModel
                             {
                                 CreatedOn = Album.CreatedOn,
                                 Id = Album.Id,
                                 NoOfPhotos = _context.AlbumDocumentMaps.Where(a => a.AlbumsId == Album.Id).Count(),
                                 Title = Album.Name
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
                    finallist = finallist.Where(p => p.CreatedOn == Convert.ToDateTime(l));
                }

                #endregion

                int TotalCount = finallist.Count();

                #region Sorting

                string SortColumn = parameters["sort_by"].ToString();
                string SortDir = "";//parameters["sort_order"].ToString();
                //"Id", "Title", "Date", "NoOfPhotos", "Action"
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
                            finallist = finallist.OrderByDescending(x => x.CreatedOn);
                        }
                        else
                        {
                            finallist = finallist.OrderBy(x => x.CreatedOn);
                        }
                        break;
                    case "NoOfPhotos":
                        if (SortDir == "desc")
                        {
                            finallist = finallist.OrderByDescending(x => x.NoOfPhotos);
                        }
                        else
                        {
                            finallist = finallist.OrderBy(x => x.NoOfPhotos);
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
                                         model.CreatedOn.ToString("dd/MM/yyyy"),
                                         model.NoOfPhotos.ToString()

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
            var model = new GalleryViewModel();
            if(id > 0)
            {
                var rec = _context.Albums.Where(a => a.Id == id).Select(j => new GalleryViewModel
                {
                    Id = j.Id,
                    Title = j.Name
                }).FirstOrDefault();

                if (rec != null)
                    model = rec;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(GalleryViewModel model)
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
                            var rec = _context.Albums.Where(e => e.Id == model.Id).FirstOrDefault();
                            if (rec == null)
                                throw new Exception("Record not found.");

                            rec.ModifiedBy = user.Id;
                            rec.ModifiedOn = DateTime.UtcNow;

                            rec.Name = model.Title;
                            _context.Update(rec);

                            _context.SaveChanges();
                            transaction.Commit();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            var albumRec = new Album
                            {
                                CreatedBy = user.Id,
                                CreatedOn = DateTime.UtcNow,
                                Name = model.Title
                            };
                            _context.Albums.Add(albumRec);
                            _context.SaveChanges();

                            if (model.Files != null && model.Files.Count > 0 && !model.Files.Where(f => f.Length == 0).Any())
                            {
                                foreach (var item in model.Files)
                                {
                                    //Upload files
                                    var fileRes = await _documentManager.Save(item, _amazonSettings.SliderBucketName);

                                    fileRes.DocumentCategory = Enums.DocumentCategory.AlbumPhoto;
                                    fileRes.CreatedBy = user.Id;

                                    _context.Add(new AlbumDocumentMap()
                                    {
                                        CreatedBy = user.Id,
                                        CreatedOn = DateTime.UtcNow,
                                        DocumentId = fileRes.Id,
                                        AlbumsId = albumRec.Id
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
            return View("AddEdit", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var rec = _context.Albums.Where(s => s.Id == id).FirstOrDefault();
                    if (rec == null)
                        throw new Exception("Invalid Album id.");

                    var photos = _context.AlbumDocumentMaps.Where(a => a.AlbumsId == rec.Id).ToList();
                    //List<AlbumDocumentMap> removedPics = new List<AlbumDocumentMap>();
                    if(photos.Count > 0)
                    {
                        foreach (var item in photos)
                        {
                            var res = await _documentManager.Delete(item.DocumentId);
                            //removedPics.Add(item);
                        }

                        //  _context.RemoveRange(removedPics);
                        //  _context.SaveChanges();
                        var removedPics = _context.AlbumDocumentMaps.Where(m => m.AlbumsId == rec.Id).ToList();
                        _context.RemoveRange(removedPics);
                    }                   
                    _context.Remove(rec);
                    _context.SaveChanges();
                    transaction.Commit();
                    return Json(new { success = true, message = "Album deleted successfully." });

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