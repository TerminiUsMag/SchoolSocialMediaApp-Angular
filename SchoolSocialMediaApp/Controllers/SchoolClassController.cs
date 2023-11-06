using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.ViewModels.Models.SchoolClass;

namespace SchoolSocialMediaApp.Controllers
{
    public class SchoolClassController : BaseController
    {
        private readonly ISchoolClassService schoolClassService;

        public SchoolClassController(ISchoolClassService _schoolClassService)
        {
            this.schoolClassService = _schoolClassService;
        }
        [HttpGet]
        public async Task<IActionResult> ManageAll(Guid schoolId, Guid userId, string message = "", string classOfMessage = "")
        {
            ICollection<SchoolClassViewModel>? classes;
            try
            {
                classes = await schoolClassService.GetAllClassesAsync(schoolId, userId);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(HomeController.Index), new { message = "Something went wrong, try again", classOfMessage = "text-bg-danger" });
            }

            return View(classes);
        }
        [HttpGet]
        public async Task<IActionResult> Manage(Guid classId)
        {
            var schoolClass = await schoolClassService.GetClassByIdAsync(classId);

            return View(schoolClass);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new SchoolClassCreateModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SchoolClassCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = this.GetUserId();
            try
            {
                await schoolClassService.CreateSchoolClassAsync(model, userId);
            }
            catch (ArgumentException ae)
            {
                ModelState.AddModelError("", ae.Message);
                return View(model);
            }

            return RedirectToAction(nameof(ManageAll), new { schoolId = Guid.Empty, userId = userId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid classId)
        {
            await schoolClassService.DeleteClass(classId, this.GetUserId());

            return RedirectToAction(nameof(ManageAll), new { userId = this.GetUserId(), message = "Class deleted successfully", classOfMessage = "text-bg-success" });
        }
    }
}
