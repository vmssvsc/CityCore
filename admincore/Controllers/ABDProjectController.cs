using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace admincore.Controllers
{
    public class ABDProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ABDProjects()
        {
            return View();
        }
        public IActionResult ABDinitiatives()
        {
            return View();
        }

        public IActionResult InitiativesAddEdit()
        {
            return View();
        }
    }
}