using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.School;
using SchoolSocialMediaApp.ViewModels.Models.User;

namespace SchoolSocialMediaApp.Areas.Admin.Controllers
{
    public class AdminController : BaseAdminController
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAccountService accountService;
        private readonly IRoleService roleService;
        private readonly ISchoolService schoolService;

        public AdminController(IAccountService _accountService, UserManager<ApplicationUser> _userManager, IRoleService _roleService, ISchoolService _schoolService, SignInManager<ApplicationUser> _signInManager)
        {
            this.accountService = _accountService;
            this.userManager = _userManager;
            this.roleService = _roleService;
            this.schoolService = _schoolService;
            this.signInManager = _signInManager;
        }


        [HttpGet]
        [Area("Admin")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AdminPanel(string message = "", string classOfMessage = "")
        {
            try
            {
                var userId = this.GetUserId();
                AdminPanelViewModel model = await accountService.GetAdminPanelViewModel(userId);
                ViewBag.Message = message;
                ViewBag.ClassOfMessage = classOfMessage;
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home", new { message = ex.Message, classOfMessage = "text-bg-danger" });
            }
        }

        [HttpGet]
        [Area("Admin")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id, string message = "", string classOfMessage = "")
        {
            var userId = this.GetUserId();

            if (!await roleService.UserIsInRoleAsync(userId.ToString(), "Admin"))
            {
                return RedirectToAction("AccessDenied","Account", new { msg = "You are not authorized to delete users" });
            }

            AdminUserDeletionViewModel model = await accountService.GetAdminUserDeletionViewModelAsync(id);

            ViewBag.Message = message;
            ViewBag.ClassOfMessage = classOfMessage;

            return View(model);
        }

        [HttpPost]
        [Area("Admin")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteUser(AdminUserDeletionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something went wrong!");
                return View(model);
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
                    return RedirectToAction("AdminPanel", "Admin", new { message = "Wrong Password", classOfMessage = "text-bg-danger" });
                }
                if (await roleService.UserIsInRoleAsync(model.Id.ToString(), "Principal"))
                {
                    return RedirectToAction("AdminPanel", "Admin", new { message = "Delete school first", classOfMessage = "text-bg-danger" });
                }
                if (!await roleService.UserIsInRoleAsync(userId.ToString(), "Admin"))
                {
                    return RedirectToAction("Index", "Home", new { message = "You are not authorized to delete accounts :) !", classOfMessage = "text-bg-danger" });
                }
                var userToDelete = await userManager.FindByIdAsync(model.Id.ToString());
                await accountService.DeleteAsync(userToDelete.Id);
                await signInManager.RefreshSignInAsync(user);
            }
            catch (Exception ex)
            {
                return RedirectToAction("AdminPanel", "Admin", new { message = ex.Message, classOfMessage = "text-bg-danger" });
            }

            return RedirectToAction("AdminPanel", "Admin", new { message = "Successfully deleted account !", classOfMessage = "text-bg-success" });
        }

        [HttpGet]
        [Area("Admin")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> MakeAdmin(Guid id, string message = "", string classOfMessage = "")
        {
            var userId = this.GetUserId();

            if (!await roleService.UserIsInRoleAsync(userId.ToString(), "Admin"))
            {
                return RedirectToAction("AccessDenied", "Account", new { msg = "You are not authorized to delete users" });
            }

            MakeUserAdminViewModel model = await accountService.GetMakeUserAdminViewModelAsync(id);

            ViewBag.Message = message;
            ViewBag.ClassOfMessage = classOfMessage;

            return View(model);
        }

        [HttpPost]
        [Area("Admin")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> MakeAdmin(MakeUserAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something went wrong!");
                return View(model);
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
                    return RedirectToAction("AdminPanel", "Admin", new { message = "Wrong Password", classOfMessage = "text-bg-danger" });
                }
                if (await roleService.UserIsInRoleAsync(model.Id.ToString(), "Principal"))
                {
                    return RedirectToAction("AdminPanel", "Admin", new { message = "Delete school first", classOfMessage = "text-bg-danger" });
                }
                if (!await roleService.UserIsInRoleAsync(userId.ToString(), "Admin"))
                {
                    return RedirectToAction("Index", "Home", new { message = "You are not authorized to give admin permissions :) !", classOfMessage = "text-bg-danger" });
                }
                var userToMakeAdmin = await userManager.FindByIdAsync(model.Id.ToString());
                await accountService.MakeAdmin(userToMakeAdmin);
                await signInManager.RefreshSignInAsync(user);
            }
            catch (Exception ex)
            {
                return RedirectToAction("AdminPanel", "Admin", new { message = ex.Message, classOfMessage = "text-bg-danger" });
            }

            return RedirectToAction("AdminPanel", "Admin", new { message = "Successfully given administrator permissions !", classOfMessage = "text-bg-success" });
        }


        //SchoolController Actions 

        [HttpGet]
        [Area("Admin")]
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
        [Area("Admin")]
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
                return RedirectToAction("AdminSchoolManage", "Admin", new { schoolId = model.Id, message = ex.Message, classOfMessage = "text-bg-danger" });
            }

            return RedirectToAction("AdminSchoolManage", "Admin", new { schoolId = model.Id, message = "Updated successfully", classOfMessage = "text-bg-success" });
        }

        [HttpGet]
        [Area("Admin")]
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
        [Area("Admin")]
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
                    return RedirectToAction("DeleteAsAdmin", "Admin", new { schoolId = model.Id, message = "Wrong Password", classOfMessage = "text-bg-danger" });
                }
                var isAdmin = await roleService.UserIsInRoleAsync(userId.ToString(), "Admin");
                if (!isAdmin)
                {
                    return RedirectToAction("Manage", "Account", new { message = "You are not Admin", classOfMessage = "text-bg-danger" });
                }

                var school = await schoolService.GetSchoolByIdAsync(model.Id);
                if (school is null)
                {
                    return RedirectToAction("AdminPanel", "Admin", new { message = $"There's no school with Id: {model.Id}", classOfMessage = "text-bg-danger" });
                }

                await schoolService.DeleteSchoolAsync(school.Id);
                await signInManager.RefreshSignInAsync(user);
            }
            catch (Exception ex)
            {
                return RedirectToAction("AdminSchoolManage", "Admin", new { schoolId = model.Id, message = ex.Message, classOfMessage = "text-bg-danger" });
            }

            return RedirectToAction("AdminPanel", "Admin", new { message = $"{model.Name} Deleted successfully!", classOfMessage = "text-bg-success" });
        }
    }
}
