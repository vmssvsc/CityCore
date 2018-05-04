using Microsoft.AspNetCore.Identity;
using CityCore.Services;
using Microsoft.Extensions.Logging;
using CityCore.Data;
using CityCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CityCore.Controllers
{
    public class TeamController : BaseController
    {

        public TeamController(UserManager<ApplicationUser> userManager,
         SignInManager<ApplicationUser> signInManager,
         IEmailSender emailSender,
         ILogger<AccountController> logger, ApplicationDbContext context) : base(userManager, signInManager, emailSender, logger, context)
        {

        }


          public IActionResult Index()
        {
            var query = _context.TeamMembers.OrderBy(k => k.CreatedOn).Select(s => new TeamMemberViewModel()
            {
                Id = s.Id,
                Name = s.Name,
                Post = s.Post,
                Image = _context.Documents.Where(d => d.Id == s.ImageDocumentId).Select(j => j.URL).FirstOrDefault(),
            }).ToList();
            return View(query);



        }
    }
}