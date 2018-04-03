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

        public IActionResult Albums()
        {
            var query = _context.Albums.OrderBy(k => k.CreatedOn).Select(s => new AlbumViewModel()
            {
                Albumname = s.Name,
                CoverImage = _context.AlbumDocumentMaps.Where(d =>d.AlbumsId == s.Id).OrderByDescending(k => k.CreatedOn).Select(l => l.Document.URL).FirstOrDefault(),
                CreatedOn = s.CreatedOn,
                NoOfPhotos = _context.AlbumDocumentMaps.Where(m => m.AlbumsId == s.Id).Count(),
                Id = s.Id
            }).OrderByDescending(g => g.CreatedOn).ToList();
            return View(query);
        }


        public IActionResult AlbumPhotos(int albumId)
        {
            var query = (from a in _context.AlbumDocumentMaps
                         join d in _context.Documents
                         on a.DocumentId equals d.Id
                         where a.AlbumsId == albumId
                         select new PhotoViewModel
                         {
                             CreatedOn = d.CreatedOn,
                             Id = d.Id,
                             Photoname = d.FileName,
                             Url = d.URL
                         }).OrderByDescending(a => a.CreatedOn).ToList();
            ViewBag.AlbumName = _context.Albums.Where(j => j.Id == albumId).Select(n => n.Name).FirstOrDefault();

            return View(query);
        }


        public IActionResult Media()
        {
            var query = _context.Videos.OrderBy(v => v.CreatedOn).Select(s => new MediaViewModel()
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                VideoUrl = s.URL                
            }).ToList();
            return View(query);

        }
    }
}