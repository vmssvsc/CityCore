using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CityCore.Models;
using Microsoft.AspNetCore.Identity;
using CityCore.Services;
using Microsoft.Extensions.Logging;
using CityCore.Data;

namespace CityCore.Controllers
{
    public class EventsController : BaseController
    {
        public EventsController(UserManager<ApplicationUser> userManager,
         SignInManager<ApplicationUser> signInManager,
         IEmailSender emailSender,
         ILogger<AccountController> logger, ApplicationDbContext context) : base(userManager, signInManager, emailSender, logger, context)
        {

        }

        public IActionResult Index()
        {
            var query = _context.Events.OrderBy(k => k.EventDate).Select(s => new EventViewModel()
            {
                Date = s.EventDate,
                Description = s.Description,
                Image = _context.Documents.Where(d => d.Id == s.ImageDocumentId).Select(j => j.URL).FirstOrDefault(),
                File = _context.Documents.Where(d => d.Id == s.DocumentId).Select(j => j.URL).FirstOrDefault(),
                Id = s.Id,
                Title = s.Title
            }).ToList();
            return View(query);
        }

        public IActionResult Photos()
        {
            return View();
        }

        public IActionResult Media()
        {
            return View();
        }
    }
}