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

        public  IActionResult Index()
        {
            return View();
        }



        public IActionResult AddEdit()
        {
            return View();
        }


        public IActionResult Save()
        {
            return View();
        }


        public IActionResult Delete()
        {
            return View();
        }

        public IActionResult DeleteDocument()
        {
            return View();
        }


    }
}