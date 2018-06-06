using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using admincore.Models;
using admincore.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using admincore.Data;
using Microsoft.Extensions.Options;
using admincore.Common;
using System.Linq;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using admincore.Data.Models;

namespace admincore.Controllers
{

    [Authorize]
    public class TenderController : BaseController
    {
        public TenderController(UserManager<ApplicationUser> userManager,
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

    }
}