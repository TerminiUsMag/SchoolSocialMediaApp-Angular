using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.ViewModels.Models;

namespace SchoolSocialMediaApp.Controllers
{
    public class SchoolController : BaseController
    {
        private readonly ISchoolService schoolService;

        public SchoolController(ISchoolService _schoolService)
        {
            this.schoolService = _schoolService;
        }

        [HttpGet]
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
        public IActionResult Register()
        {
            var model = new SchoolViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(SchoolViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = this.GetUserId();
            if(userId == Guid.Empty)
            {
                return RedirectToAction("Login", "Account");
            }
            model.ImageUrl = "~/Default/defaultSchool.png";
            try
            {
                await schoolService.CreateSchoolAsync(model, userId);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }

            return RedirectToAction("Success");
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
    }
}
