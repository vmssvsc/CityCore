using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CityCore.Controllers
{
    public class TenderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}