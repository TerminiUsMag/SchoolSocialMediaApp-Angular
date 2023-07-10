using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models;

namespace SchoolSocialMediaApp.Controllers
{
    public class SchoolController : BaseController
    {
        private readonly ISchoolService schoolService;
        private readonly IInvitationService invitationService;
        private readonly UserManager<ApplicationUser> userManager;

        public SchoolController(ISchoolService _schoolService,IInvitationService _invitationService,UserManager<ApplicationUser> _userManager)
        {
            this.schoolService = _schoolService;
            this.invitationService = _invitationService;
            this.userManager = _userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<SchoolViewModel>? schools = null;
            try
            {
                schools = await schoolService.GetAllSchoolsAsync();
            }
            catch (ArgumentException)
            {
                schools = new List<SchoolViewModel>();
            }

            return View(schools);
        }

        [HttpGet]
        //[Authorize(Roles = "User")]
        public IActionResult Register()
        {
            var model = new SchoolViewModel();

            return View(model);
        }

        [HttpPost]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> Register(SchoolViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid school data.");
                return View(model);
            }

            var userId = this.GetUserId();
            if (userId == Guid.Empty)
            {
                return RedirectToAction("Login", "Account");
            }
            model.ImageUrl = "/images/defaultSchool.png";
            try
            {
                model = await schoolService.CreateSchoolAsync(model, userId);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }

            return RedirectToAction("Success", model);
        }

        [HttpGet]
        [Authorize(Roles = "Principal")]
        public IActionResult Success(SchoolViewModel model)
        {
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            var model = await schoolService.GetSchoolByIdAsync(id);

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "Principal")]
        public async Task<IActionResult> Manage(string message)
        {
            var userId = this.GetUserId();
            if (userId == Guid.Empty)
            {
                return RedirectToAction("Login", "Account");
            }

            SchoolManageViewModel? school = null;

            try
            {
                school = await schoolService.GetSchoolManageViewModelByUserIdAsync(userId);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            ViewBag.Message = message;
            return View(school);
        }

        
    }
}
