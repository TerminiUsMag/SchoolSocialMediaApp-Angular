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
        public async Task<IActionResult> Index(string searchString)
        {
            IEnumerable<SchoolViewModel>? schools = null;
            try
            {
                schools = await schoolService.GetAllSchoolsAsync();
                if(!string.IsNullOrEmpty(searchString))
                {
                    schools = schools.Where(s=>s.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase));
                    ViewBag.SearchString = searchString;
                }
            }
            catch (ArgumentException)
            {
                schools = new List<SchoolViewModel>();
            }

            return View(schools);
        }

        [HttpGet]
        [Authorize(Policy = "CanBePrincipal")]
        public IActionResult Register(string message = "", string classOfMessage = "")
        {
            var model = new SchoolViewModel();
            ViewBag.Message = message;
            ViewBag.ClassOfMessage = classOfMessage;

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
                return RedirectToAction("Register", new { message = e.Message, classOfMessage = "text-bg-danger" });
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
        public async Task<IActionResult> Manage(string message = "", string classOfMessage = "text-bg-danger")
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
                await schoolService.UpdateSchoolAsync(model, userId);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Manage", "School", new { message = ex.Message, classOfMessage = "text-bg-danger" });
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
            var thisUserId = this.GetUserId();
            var user = await userManager.FindByIdAsync(thisUserId.ToString());
            var userIsAdmin = await roleService.UserIsInRoleAsync(thisUserId.ToString(), "Admin");
            var userIsPrincipal = await roleService.UserIsInRoleAsync(thisUserId.ToString(), "Principal");

            if (!userIsAdmin && !userIsPrincipal)
            {
                return RedirectToAction("Index", "Home", new { message = "You are not authorized to remove users from this school", classOfMessage = "text-bg-danger" });
            }

            try
            {
                await schoolService.RemoveUserFromSchoolAsync(userId, schoolId);
                await signInManager.RefreshSignInAsync(user);
            }
            catch (Exception e)
            {
                return RedirectToAction("Manage", new { message = e.Message });
            }
            if (userIsAdmin)
            {
                return RedirectToAction("AdminSchoolManage", new { schoolId = schoolId, message = "User removed from school successfully", classOfMessage = "text-bg-success" });
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

                await schoolService.DeleteSchoolAsync(school.Id);
                await signInManager.RefreshSignInAsync(user);

            }
            catch (Exception ex)
            {
                return RedirectToAction("Manage", "Account", new { message = ex.Message, classOfMessage = "text-bg-danger" });
            }

            return RedirectToAction("Index", "Home", new { message = "School deleted successfully !", classOfMessage = "text-bg-success" });
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AdminSchoolManage(Guid schoolId, string message = "", string classOfMessage = "")
        {
            var userId = this.GetUserId();
            var userIsAdmin = await roleService.UserIsInRoleAsync(userId.ToString(), "Admin");


            try
            {
                if (!userIsAdmin)
                {
                    throw new InvalidOperationException("You are not admin!");
                }

                SchoolManageViewModel? model = await schoolService.GetSchoolManageViewModelBySchoolIdAsync(schoolId);
                ViewBag.Message = message;
                ViewBag.ClassOfMessage = classOfMessage;
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("AccessDenied", "Account", new { msg = ex.Message });
            }

        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AdminSchoolManage(SchoolManageViewModel model)
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
                //return RedirectToAction("AdminSchoolManage", "School", new { schoolId = model.Id, message = "Something went wrong", classOfMessage = "text-bg-danger" });
            }

            try
            {
                await schoolService.UpdateSchoolAsync(model, userId);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("AdminSchoolManage", "School", new { schoolId = model.Id, message = ex.Message, classOfMessage = "text-bg-danger" });
            }

            return RedirectToAction("AdminSchoolManage", new { schoolId = model.Id, message = "Updated successfully", classOfMessage = "text-bg-success" });
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteAsAdmin(Guid schoolId, string message = "", string classOfMessage = "")
        {
            var userId = this.GetUserId();
            var userIsAdmin = await roleService.UserIsInRoleAsync(userId.ToString(), "Admin");

            try
            {
                if (!userIsAdmin)
                {
                    throw new InvalidOperationException("You are not admin!");
                }
                AdminSchoolDeleteViewModel model = await schoolService.GetAdminSchoolDeleteViewBySchoolIdAsync(schoolId);
                ViewBag.Message = message;
                ViewBag.ClassOfMessage = classOfMessage;
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("AccessDenied", "Account", new { msg = ex.Message });
            }

        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteAsAdmin(AdminSchoolDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
                //return RedirectToAction("DeleteAsAdmin", "School", new {schoolId = model.Id, message = "Something went wrong", classOfMessage = "text-bg-danger" });
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
                    return RedirectToAction("DeleteAsAdmin", "School", new { schoolId = model.Id, message = "Wrong Password", classOfMessage = "text-bg-danger" });
                }
                var isAdmin = await roleService.UserIsInRoleAsync(userId.ToString(), "Admin");
                if (!isAdmin)
                {
                    return RedirectToAction("Manage", "Account", new { message = "You are not Admin", classOfMessage = "text-bg-danger" });
                }

                var school = await schoolService.GetSchoolByIdAsync(model.Id);
                if (school is null)
                {
                    return RedirectToAction("AdminPanel", "Account", new { message = $"There's no school with Id: {model.Id}", classOfMessage = "text-bg-danger" });
                }

                await schoolService.DeleteSchoolAsync(school.Id);
                await signInManager.RefreshSignInAsync(user);
            }
            catch (Exception ex)
            {
                return RedirectToAction("AdminSchoolManage", "School", new { schoolId = model.Id, message = ex.Message, classOfMessage = "text-bg-danger" });
            }

            return RedirectToAction("AdminPanel", "Account", new { message = $"{model.Name} Deleted successfully!", classOfMessage = "text-bg-success" });
        }
    }
}

