using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace admincore.Controllers
{
    public class PanCityProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PancityTable()
        {
            return View();
        }

        public IActionResult PancityCommand()
        {
            return View();
        }
        public IActionResult CommandAddEdit()
        {
            return View();
        }

    }
}