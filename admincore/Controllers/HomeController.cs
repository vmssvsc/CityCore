using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using admincore.Models;
using admincore.Services;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace admincore.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController(UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           IEmailSender emailSender,
           ILogger<AccountController> logger) : base(userManager, signInManager, emailSender, logger)
        {
        }

       
        public async Task<IActionResult> Index() 
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return View();
            else
                await SetUserData();
            return View("~/Views/Home/MenuDescription.cshtml");

        }
        [Authorize]
        public async Task<IActionResult> MenuDescription()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return View();
            else
                await SetUserData();
            return View();

        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
