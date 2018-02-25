using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using admincore.Models;
using admincore.Services;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Data.Common;
using admincore.Data;
using System.Linq;
using System.Collections.Generic;
using admincore.Data.Models;
using Microsoft.Extensions.Options;
using admincore.Common;

namespace admincore.Controllers
{
    [Authorize]
    public class SliderController : BaseController
    {
        public SliderController(UserManager<ApplicationUser> userManager,
         SignInManager<ApplicationUser> signInManager,
         IEmailSender emailSender,
         ILogger<AccountController> logger, IDocumentManager documentManager, ApplicationDbContext context, IOptions<AmazonSettings> amazonSettings) : base(userManager, signInManager, emailSender, logger, documentManager, context, amazonSettings)
        {
        }

        public async Task<IActionResult> Index()
        {         
            await SetUserData();

            var NoOfSliders = 5;//_context.Settings.Where(e => e.EnumValue == Enums.SettingsValues.NoOfSliderImages).Select(k => k.SettingValue).FirstOrDefault();

            ViewBag.NoOfSliders = Convert.ToInt16(NoOfSliders);

            var sliderList = (from slider in _context.SliderImages
                              join doc in _context.Documents
                              on slider.DocumentId equals doc.Id
                              where slider.SequenceNo > 0
                              select new SliderViewModel()
                              {
                                  Id = slider.Id,
                                  FileName = doc.FileName,
                                  Url = doc.URL,
                                  SequenceNumber = slider.SequenceNo
                              }).ToList();

            return View(sliderList);
        }

        [HttpPost]
        public async Task<IActionResult> Save(int sequenceNo, IFormFile file)
        {   
            using (var transaction = _context.Database.BeginTransaction() )
            {
                try
                {
                    if (file != null && file.Length > 0)
                    {
                        //upload.  TODo Check if file size is > that defined in settings table

                        if (sequenceNo == 0)
                            throw new Exception("Sequence number can not be zero.");

                        if (_context.SliderImages.Where(s => s.SequenceNo == sequenceNo).Any())
                            throw new Exception("Image already exists at this sequence number.");

                        var res = await _documentManager.Save(file, _amazonSettings.SliderBucketName); 

                        if (res != null)
                        {
                            var user = await _userManager.GetUserAsync(User);
                            res.DocumentCategory = Common.Enums.DocumentCategory.SliderImage;
                            res.CreatedBy = user.Id;

                            _context.Add(new SliderImage()
                            {
                                CreatedBy = user.Id,
                                CreatedOn = DateTime.UtcNow,
                                DocumentId = res.Id,
                                SequenceNo = sequenceNo,
                            });

                            _context.SaveChanges();
                            transaction.Commit();
                            return Json(new { success = true, url = res.URL, id = res.Id, name = res.FileName, message = "File uploaded successfully." });
                        }
                        else
                        {
                            transaction.Rollback();
                            return Json(new { success = false, message = "Upload failed." });
                        }                          
                    }
                    else
                        return Json(new { success = false, message = "Please select a file." });
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return Json(new { success = false, message = e.Message });
                }
            }           
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int sequenceNumber)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var rec = _context.SliderImages.Where(s => s.SequenceNo == sequenceNumber).FirstOrDefault();
                    if (rec == null)
                        throw new Exception("Invalid sequence number.");

                    var res = await _documentManager.Delete(rec.DocumentId);

                    if (res)
                    {
                        var user = await _userManager.GetUserAsync(User);
                        _context.Remove(rec);
                        transaction.Commit();
                        return Json(new { success = true, message = "File deleted successfully." });
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

        //[HttpGet]
        //public async Task<IActionResult> GetSliderImagesStatus(int Id)
        //{
        //    var docs = _context.SliderImages.Select(k => new
        //    {
        //        sequenceNumber = k.SequenceNo,
        //        URL = k.
        //    }).ToList();
        //    return Json(new { success = true, url = "", id = "" });
        //}


    }
}