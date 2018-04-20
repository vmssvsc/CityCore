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
using CityCore.Common;

namespace CityCore.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(UserManager<ApplicationUser> userManager,
         SignInManager<ApplicationUser> signInManager,
         IEmailSender emailSender,
         ILogger<AccountController> logger, ApplicationDbContext context) : base(userManager, signInManager, emailSender, logger, context)
        {

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Welcome()
        {
            var model = new WelcomeViewModel();
            model.SliderImages = (from s in _context.SliderImages
                                  join d in _context.Documents
                                  on s.DocumentId equals d.Id
                                  select new SliderImage()
                                  {
                                      SequenceNumber = s.SequenceNo,
                                      Url = d.URL
                                  }).ToList();

            model.SmartProject = (from p in _context.SmartCityProjects
                                  join d in _context.Documents
                                  on p.DocumentId equals d.Id
                                  where p.DisplayLocation == Common.Enums.SmartCityProjectDisplayLocation.Home
                                  select new SmartProjectViewModel()
                                  {
                                      Description = p.Description,
                                      Id = p.Id,
                                      Name = p.Name,
                                      Url = p.Url,
                                      ImageUrl = d.URL
                                  }).ToList();

            model.News = (from p in _context.News
                          where p.Status == Enums.NewsStatus.Active
                          select new NewsViewModel
                          {
                              CoverURL = (from m in _context.NewsDocumentMaps
                                          join d in _context.Documents
                                          on m.DocumentId equals d.Id
                                          where m.NewsId == p.Id
                                          orderby m.CreatedOn descending
                                          select d.URL).FirstOrDefault(),
                              Date = DateTimeExtensions.ToShortMonthName(p.Date) + p.Date.Day + "," + p.Date.Year,
                              Description = p.Description,
                              NewsType = p.NewsType,
                              Title = p.Title,
                              Id = p.Id,
                              NewsPriority = p.Priority,
                              DateTime = p.Date
                          }).ToList();


            return View(model);
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
