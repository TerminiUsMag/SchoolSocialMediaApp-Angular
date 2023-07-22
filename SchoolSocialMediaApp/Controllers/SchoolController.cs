using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.School;

namespace SchoolSocialMediaApp.Controllers
{
    public class SchoolController : BaseController
    {
        private readonly ISchoolService schoolService;
        private readonly IInvitationService invitationService;
        private readonly UserManager<ApplicationUser> userManager;

        public SchoolController(ISchoolService _schoolService, IInvitationService _invitationService, UserManager<ApplicationUser> _userManager)
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
        [Authorize(Policy = "CanBePrincipal")]
        public IActionResult Register()
        {
            var model = new SchoolViewModel();

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "CanBePrincipal")]
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
        [Authorize(Policy = "Principal")]
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
        public async Task<IActionResult> Manage(string message, string classOfMessage = "text-bg-danger")
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
                ViewBag.Code = 404;
            }
            ViewBag.Message = message;
            ViewBag.ClassOfMessage = classOfMessage;
            return View(school);
        }

        [HttpPost]
        [Authorize(Policy = "Principal")]
        public async Task<IActionResult> Manage(SchoolManageViewModel model)
        {
            var userId = this.GetUserId();
            if (userId == Guid.Empty)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
            }

            try
            {
                await schoolService.UpdateSchoolAsync(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }

            return RedirectToAction("Manage", "School", new { message = "Updated successfully", classOfMessage = "text-bg-success" });
        }

        [HttpPost]
        [Authorize(Policy = "Principal")]
        public async Task<IActionResult> Remove(Guid userId, Guid schoolId)
        {
            if (userId == Guid.Empty || schoolId == Guid.Empty)
            {
                return RedirectToAction("Manage", new { message = "Invalid user or school id." });
            }

            try
            {
                await schoolService.RemoveUserFromSchoolAsync(userId, schoolId);
            }
            catch (Exception e)
            {
                return RedirectToAction("Manage", new { message = e.Message });
            }

            return RedirectToAction("Manage", new { message = "User removed successfully.", classOfMessage = "text-bg-success" });
        }

        [HttpPost]
        public async Task<IActionResult> Rename(Guid schoolId, string schoolName)
        {
            try
            {
                await schoolService.RenameSchoolAsync(schoolId, schoolName);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, errorMsg = ex.Message });
            }

        }

        [HttpPost]
        public async Task<IActionResult> UploadPicture()
        {
            try
            {
                // Get the uploaded file from the request
                var file = Request.Form.Files[0];
                var fileExtension = Path.GetExtension(file.FileName);

                // Check if a file was uploaded
                if (file == null || file.Length == 0 || (fileExtension != ".jpg" && fileExtension != ".png"))
                {
                    return BadRequest("No file uploaded.");
                }

                // Create a unique file name to save the uploaded image
                var fileName = Guid.NewGuid().ToString() + fileExtension;

                // Specify the directory where the image will be saved
                var imagePath = Path.Combine("wwwroot", "images", "school-images", fileName);

                // Save the file to the specified path
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                //Change the Image url in the database
                var userId = this.GetUserId();

                await schoolService.ChangeSchoolPicture(userId, $"images/school-images/{fileName}");

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the upload process
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}

