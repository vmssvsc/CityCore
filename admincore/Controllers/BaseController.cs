﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using admincore.Models;
using admincore.Services;
using System.Threading.Tasks;
using admincore.Data;
using System.Collections.Generic;
using System;
using admincore.Common;
using Microsoft.Extensions.Options;

namespace admincore.Controllers
{
    public class BaseController : Controller
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly SignInManager<ApplicationUser> _signInManager;
        protected readonly IEmailSender _emailSender;
        protected readonly ILogger _logger;
        protected readonly IDocumentManager _documentManager;
        protected readonly ApplicationDbContext _context;
        protected readonly AmazonSettings _amazonSettings;

        public BaseController() { }

        public BaseController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           IEmailSender emailSender,
           ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        public BaseController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IEmailSender emailSender,
          ILogger<AccountController> logger,
          IDocumentManager documentManager,
          ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _documentManager = documentManager;
            _context = context;
        }

        public BaseController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IEmailSender emailSender,
          ILogger<AccountController> logger,
          IDocumentManager documentManager,
          ApplicationDbContext context,
          IOptions<AmazonSettings> amazonSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _documentManager = documentManager;
            _context = context;
            _amazonSettings = amazonSettings.Value;
        }

        protected Dictionary<string, string> GetParameters(List<string> columns)
        {
            var queryStrings = Request.Query;
            var sortingColumn = columns[Convert.ToInt32(queryStrings["iSortCol_0"])];
            var sortDir = queryStrings["sSortDir_0"];
            var pageStart = Convert.ToInt32(queryStrings["iDisplayStart"]);
            var pageSize = Convert.ToInt32(queryStrings["iDisplayLength"]) == 0 ? 10 : Convert.ToInt32(queryStrings["iDisplayLength"]);
            var pageIndex = pageStart == 0 ? 1 : (pageStart / pageSize) + 1;
            var searchBy = System.Net.WebUtility.UrlDecode(queryStrings["srchBy"]);
            var searchTxt = System.Net.WebUtility.UrlDecode(queryStrings["srchTxt"]);
            var ToDate = System.Net.WebUtility.UrlDecode(queryStrings["ToDate"]);
            var FromDate = System.Net.WebUtility.UrlDecode(queryStrings["FromDate"]);
            //var carrierVal = queryStrings["CarrierGuid"];
            var parameters = new Dictionary<string, string>
                {
                    {"sort_by", sortingColumn},
                    {"sort_order", sortDir},
                    {"results_per_page", pageSize.ToString()},
                    {"page_number", pageIndex.ToString()},
                    {"PageStart", pageStart.ToString()},
                };
            if (!string.IsNullOrWhiteSpace(searchBy))
            {

                parameters.Add("searchBy", string.IsNullOrEmpty(searchTxt) ? string.Empty : searchTxt.ToString().ToLower());
            }
            if (!string.IsNullOrWhiteSpace(ToDate))
            {
                //searchTxt = CheckIfDate(searchTxt);

                parameters.Add("ToDate", string.IsNullOrEmpty(ToDate) ? string.Empty : ToDate.ToString().ToLower());
            }
            if (!string.IsNullOrWhiteSpace(FromDate))
            {
                //searchTxt = CheckIfDate(searchTxt);

                parameters.Add("FromDate", string.IsNullOrEmpty(FromDate) ? string.Empty : FromDate.ToString().ToLower());
            }

            return parameters;
        }

        protected async Task SetUserData()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
            }

        }
    }
}