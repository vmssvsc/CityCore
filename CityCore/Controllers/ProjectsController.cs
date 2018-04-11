using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CityCore.Models.Projects;
using CityCore.Models;
using Microsoft.AspNetCore.Identity;
using CityCore.Services;
using Microsoft.Extensions.Logging;
using CityCore.Data;

namespace CityCore.Controllers
{
    public class ProjectsController : BaseController
    {
        public ProjectsController(UserManager<ApplicationUser> userManager,
         SignInManager<ApplicationUser> signInManager,
         IEmailSender emailSender,
         ILogger<AccountController> logger, ApplicationDbContext context) : base(userManager, signInManager, emailSender, logger, context)
        {

        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Abd()
        {
            var query = _context.ProjectInitiatives.OrderBy(v => v.CreatedOn).Select(s => new ABDInitiativeViewModel()
            {
                Id = s.Id,
                Title = s.Title,
                Initiative = s.Initiative
            }).ToList();
            return View(query);

        }

        public IActionResult ProjectStatus()
        {
            return View();
        }
    }
}