using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Controllers;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.School;
using SchoolSocialMediaApp.ViewModels.Models.User;
using X.PagedList;

namespace SchoolSocialMediaApp.Areas.Admin.Controllers
{
    public class AdminController : BaseController
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAccountService accountService;
        private readonly IRoleService roleService;
        private readonly ISchoolService schoolService;
        private readonly IPostService postService;

        public AdminController(SignInManager<ApplicationUser> _signInManager, UserManager<ApplicationUser> _userManager, IRoleService _roleService, ISchoolService _schoolService, IAccountService _accountService, IPostService _postService)
        {
            this.accountService = _accountService;
            this.userManager = _userManager;
            this.roleService = _roleService;
            this.schoolService = _schoolService;
            this.signInManager = _signInManager;
            this.postService = _postService;
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
                model.Roles = await roleService.GetRolesAsync();
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
                return RedirectToAction("AccessDenied", "Account", new { msg = "You are not authorized to delete users" });
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

                SchoolManageViewModel model = await schoolService.GetSchoolManageViewModelBySchoolIdAsync(schoolId);
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

        //PostController Actions

        [HttpGet]
        [Area("Admin")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AdminPostView(Guid schoolId, int? page, string message = "", string classOfMessage = "")
        {
            var userId = this.GetUserId();
            var model = await postService.GetAllPostsAsync(schoolId, userId);
            var school = await schoolService.GetSchoolByIdAsync(schoolId);
            var schoolName = school.Name;
            ViewBag.SchoolName = schoolName;
            ViewBag.Message = message;
            ViewBag.ClassOfMessage = classOfMessage;
            ViewBag.SchoolId = schoolId;
            int pageSize = 3;
            int pageNumber = page ?? 1;
            var pagedPosts = model.ToPagedList(pageNumber, pageSize);

            return View(pagedPosts);
        }

        [HttpPost]
        [Area("Admin")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AddUserToRole(Guid userId, AdminPanelViewModel model, string message = "", string classOfMessage = "")
        {
            try
            {
                Guid roleId = Guid.Empty;
                //if (!ModelState.IsValid)
                //{
                //    throw new ArgumentException("Something went wrong");
                //}

                if (model.SelectedRoleId is not null && model.SelectedRoleId != string.Empty)
                    Guid.TryParse(model.SelectedRoleId, out roleId);

                if (roleId == Guid.Empty)
                    throw new ArgumentException("The selected role is not valid");
                var roles = await roleService.GetRolesAsync();
                foreach (var role in roles)
                {
                    await roleService.RemoveUserFromRoleAsync(userId.ToString(), role.Text);
                }
                await roleService.AddUserToRoleIdAsync(userId, roleId, this.GetUserId().ToString());

            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(AdminPanel), new { message = ex.Message, classOfMessage = "text-bg-danger" });
            }


            return RedirectToAction(nameof(AdminPanel), new { message = "User added to role successfully!", classOfMessage = "text-bg-success" });
        }

        [HttpGet]
        [Area("Admin")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> ChangeSchoolPrincipal(Guid schoolId, string message = "", string classOfMessage = "")
        {
            var userId = this.GetUserId();
            var userIsAdmin = await roleService.UserIsInRoleAsync(userId.ToString(), "Admin");


            try
            {
                if (!userIsAdmin)
                {
                    throw new InvalidOperationException("You are not admin!");
                }
                SchoolChangePrincipalViewModel model = await schoolService.GetSchoolChangePrincipalViewModelBySchoolIdAsync(schoolId);
                ViewBag.Message = message;
                ViewBag.ClassOfMessage = classOfMessage;
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("AccessDenied", "Account", new { msg = ex.Message });
            }
        }
    }
}
