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

namespace admincore.Controllers
{
    [Authorize]
    public class SliderController : BaseController
    {
        public SliderController(UserManager<ApplicationUser> userManager,
         SignInManager<ApplicationUser> signInManager,
         IEmailSender emailSender,
         ILogger<AccountController> logger, IDocumentManager documentManager, ApplicationDbContext context) : base(userManager, signInManager, emailSender, logger, documentManager, context)
        {
        }

        public async Task<IActionResult> Index()
        {         
            await SetUserData();

            var sliderList = (from slider in _context.SliderImages
                              join doc in _context.Documents
                              on slider.DocumentId equals doc.Id
                              select new SliderViewModel()
                              {
                                  DocumentId = doc.Id,
                                  FileName = doc.FileName,
                                  Url = doc.URL
                              }).ToList();

            return View(sliderList);
        }

        [HttpPost]
        public async Task<IActionResult> Save(IFormFile file)
        {
            using (var transaction = _context.Database.BeginTransaction() )
            {
                try
                {
                    if (file != null && file.Length > 0)
                    {
                        //upload.  TODo Check if file size is > that defined in settings table

                        var success = _documentManager.Save(file);

                       
                    }
                    else
                        return Json(new { success = true, url = "", id = "" });
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
            }           
            return Json(new { success = true, url ="", id = ""});
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            return Json(new { success = true, url = "", id = "" });
        }

        [HttpGet]
        public async Task<IActionResult> Get(int Id)
        {
            return Json(new { success = true, url = "", id = "" });
        }


    }
}