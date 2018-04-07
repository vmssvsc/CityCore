using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CityCore.Controllers
{
    public class SmartCityinfoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }



        public IActionResult SmartCityCoverage()
        {
            return View();
        }

        public IActionResult SmartCitySelection()
        {
            return View();
        }
    }
}