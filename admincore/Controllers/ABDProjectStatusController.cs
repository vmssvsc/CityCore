﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace admincore.Controllers
{
    public class ABDProjectStatusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult AddEdit()
        {
            return View();
        }
    }
}