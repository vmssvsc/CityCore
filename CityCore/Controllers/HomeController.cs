using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CityCore.Models;

namespace CityCore.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Welcome()
        {
            var model = new WelcomeViewModel();
            model.SliderImages = new List<SliderImage>();
            for(int i=0; i<4; i++)
            {
                var img = new SliderImage()
                {
                    Url = "http://localhost:52826/images/client-logo2.png"
                };

                model.SliderImages.Add(img);
            }

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
