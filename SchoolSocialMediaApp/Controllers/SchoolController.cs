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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ISchoolService schoolService;
        private readonly IInvitationService invitationService;
        private readonly IRoleService roleService;

        public SchoolController(ISchoolService _schoolService, IInvitationService _invitationService, UserManager<ApplicationUser> _userManager, IRoleService _roleService, SignInManager<ApplicationUser> _signInManager)
        {
            this.schoolService = _schoolService;
            this.invitationService = _invitationService;
            this.userManager = _userManager;
            this.roleService = _roleService;
            this.signInManager = _signInManager;
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

        [Authorize(Policy = "Principal")]
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
        [Authorize(Policy = "Principal")]
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

        [HttpGet]
        [Authorize(Policy = "Principal")]
        public IActionResult Delete()
        {
            var model = new SchoolDeleteViewModel();
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "Principal")]
        public async Task<IActionResult> Delete(SchoolDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something went wrong!");
                return View();
            }


            try
            {
                var userId = GetUserId();
                var user = await userManager.FindByIdAsync(userId.ToString());
                if (user is null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var correctPassword = await userManager.CheckPasswordAsync(user, model.Password);
                if (!correctPassword)
                {
                    return RedirectToAction("Manage", "School", new { message = "Wrong Password", classOfMessage = "text-bg-danger" });
                }
                var isPrincipal = await roleService.UserIsInRoleAsync(userId.ToString(), "Principal");
                var isAdmin = await roleService.UserIsInRoleAsync(userId.ToString(), "Admin");
                if (!isPrincipal && isAdmin)
                {
                    return RedirectToAction("Manage", "Account", new { message = "You are not Principal of the school (use the Admin Panel)", classOfMessage = "text-bg-danger" });
                }

                var school = await schoolService.GetSchoolByUserIdAsync(userId);
                if (school is null)
                {
                    await roleService.RemoveUserFromRoleAsync(userId.ToString(), "Principal");
                    return RedirectToAction("Manage", "Account", new { message = "You are not Principal anymore(The school is already missing!)", classOfMessage = "text-bg-danger" });
                }

                var usersInSchool = await schoolService.GetAllUsersInSchool(school.Id);

                foreach (var participant in usersInSchool)
                {
                    if (participant is not null)
                    {
                        if (participant.IsParent)
                        {
                            participant.IsParent = false;
                            await roleService.RemoveUserFromRoleAsync(participant.Id.ToString(), "Parent");
                        }
                        if (participant.IsTeacher)
                        {
                            participant.IsTeacher = false;
                            await roleService.RemoveUserFromRoleAsync(participant.Id.ToString(), "Teacher");
                        }
                        if (participant.IsStudent)
                        {
                            participant.IsStudent = false;
                            await roleService.RemoveUserFromRoleAsync(participant.Id.ToString(), "Student");
                        }
                        participant.SchoolId = null;
                        participant.School = null;
                        await signInManager.RefreshSignInAsync(participant);
                    }
                }

                await schoolService.DeleteSchoolAsync(school.Id);

            }
            catch (Exception ex)
            {
                return RedirectToAction("Manage", "Account", new { message = ex.Message, classOfMessage = "text-bg-danger" });
            }

            return RedirectToAction("Index", "Home", new { message = "School deleted successfully !", classOfMessage = "text-bg-success" });
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AdminSchoolManage(Guid schoolId)
        {
            var userId = this.GetUserId();
            var userIsAdmin = await roleService.UserIsInRoleAsync(userId.ToString(), "Admin");

            if (!userIsAdmin)
            {
                throw new InvalidOperationException("You are not admin!");
            }

            SchoolManageViewModel? model = await schoolService.GetSchoolManageViewModelBySchoolIdAsync(schoolId);

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteAsAdmin(Guid schoolId)
        {
            var model = new SchoolDeleteViewModel();

            return View(model);
        }
    }
}

