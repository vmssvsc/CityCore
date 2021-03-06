﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using admincore.Models;
using admincore.Services;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace admincore.Controllers
{
    [Authorize]
    public class PagesController : BaseController
    {
        public PagesController(UserManager<ApplicationUser> userManager,
         SignInManager<ApplicationUser> signInManager,
         IEmailSender emailSender,
         ILogger<AccountController> logger) : base(userManager, signInManager, emailSender, logger)
        {
        }

        public async Task<IActionResult> Index()
        {
            await SetUserData();
            return View();
        }
    }
}