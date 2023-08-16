using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.User;

namespace SchoolSocialMediaApp.Controllers
{
    public class AccountController : BaseController
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAccountService accountService;
        private readonly IRoleService roleService;
        private readonly ISchoolService schoolService;

        public AccountController(IAccountService _accountService, UserManager<ApplicationUser> _userManager, IRoleService _roleService, ISchoolService _schoolService,SignInManager<ApplicationUser> _signInManager)
        {
            this.accountService = _accountService;
            this.userManager = _userManager;
            this.roleService = _roleService;
            this.schoolService = _schoolService;
            this.signInManager = _signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string message = "", string classOfMessage = "")
        {
            var model = new RegisterViewModel();
            try
            {
                var userId = this.GetUserId();
                if (userId != Guid.Empty)
                {
                    return RedirectToAction("Index", "Home", new { message = "You're already logged in", classOfMessage = "text-bg-danger" });
                }
                ViewBag.Message = message;
                ViewBag.ClassOfMessage = classOfMessage;
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home", new { message = "You're already logged in - " + ex.Message, classOfMessage = "text-bg-danger" });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            //Model Validation
            if (!ModelState.IsValid)
            {
                var message = "Something went wrong";
                ModelState.AddModelError("", message);
                return View(model);
            }

            //Check if the user is logged in.
            var userId = this.GetUserId();
            if (userId != Guid.Empty)
            {
                return RedirectToAction("Index", "Home", new { message = "You're already logged in", classOfMessage = "text-bg-danger" });
            }

            //Username Validation and Verification
            var username = $"{model.FirstName}.{model.LastName}".ToLower();

            if (!await accountService.UsernameIsFree(username))
            {
                var message = $"Username '{username}' is already taken";
                ModelState.AddModelError("", message);
                return RedirectToAction("Register", "Account", new { message = message, classOfMessage = "text-bg-danger" });
            }

            //Phone Number Validation and Verification
            if (!accountService.PhoneNumberIsValid(model.PhoneNumber))
            {
                var message = "Invalid Phone Number: " + model.PhoneNumber;
                ModelState.AddModelError("", message);
                return RedirectToAction("Register", "Account", new { message = message, classOfMessage = "text-bg-danger" });
            }
            if (!await accountService.PhoneNumberIsFree(model.PhoneNumber))
            {
                var message = "Phone Number is already taken: " + model.PhoneNumber;
                ModelState.AddModelError("", message);
                return RedirectToAction("Register", "Account", new { message = message, classOfMessage = "text-bg-danger" });
            }

            //Email Validation and Verification
            if (!accountService.EmailIsValid(model.Email))
            {
                var message = "Invalid Email: " + model.Email;
                ModelState.AddModelError("", message);
                return RedirectToAction("Register", "Account", new { message = message, classOfMessage = "text-bg-danger" });
            }
            if (!await accountService.EmailIsFree(model.Email))
            {
                var message = "Email is already taken: " + model.Email;
                ModelState.AddModelError("", message);
                return RedirectToAction("Register", "Account", new { message = message, classOfMessage = "text-bg-danger" });
            }


            //User Creation
            var user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = username,
                PhoneNumber = model.PhoneNumber,
                CreatedOn = DateTime.Now,
                ImageUrl = "/images/defaultProfile.png",
            };

            //User Registration and Password Hashing
            if (!await accountService.RegisterAsync(user, model.Password))
            {
                var message = "Something went wrong";
                ModelState.AddModelError("", message);
                return RedirectToAction("Register", "Account", new { message = message, classOfMessage = "text-bg-danger" });
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "/")
        {
            //Check if the user is logged in.
            var userId = this.GetUserId();
            if (userId != Guid.Empty)
            {
                return RedirectToAction("Index", "Home", new { message = "You're already logged in", classOfMessage = "text-bg-danger" });
            }

            var model = new LoginViewModel()
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //Model Validation
            if (!ModelState.IsValid)
            {
                var message = "Something went wrong";
                ModelState.AddModelError("", message);
                return View(model);
            }

            //Check if the user is logged in.
            var userId = this.GetUserId();
            if (userId != Guid.Empty)
            {
                return RedirectToAction("Index", "Home", new { message = "You're already logged in", classOfMessage = "text-bg-danger" });
            }

            var result = await accountService.LoginAsync(model.Email, model.Password, model.RememberMe = false);

            if (!result)
            {
                var message = "Something went wrong";
                ModelState.AddModelError("", message);
                return RedirectToAction("Login", "Account", new { message = message, classOfMessage = "text-bg-danger" });
            }

            //return RedirectToAction(nameof(HomeController.Index), "Home");
            return Redirect(model.ReturnUrl ?? "/");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await accountService.LogoutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [Authorize(Policy = "CanBePrincipal")]
        public IActionResult BecomePrincipal(string message = "", string classOfMessage = "")
        {
            ViewBag.Message = message;
            ViewBag.ClassOfMessage = classOfMessage;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied(string msg)
        {
            ViewBag.Error = msg;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Manage(string message, string classOfMessage)
        {
            var userId = this.GetUserId();
            if (userId == Guid.Empty)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var model = await accountService.GetUserManageViewModelAsync(userId.ToString());
            ViewBag.Message = message;
            ViewBag.ClassOfMessage = classOfMessage;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(UserManageViewModel model)
        {
            //Model Validation
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
            }

            var userId = this.GetUserId();
            if (userId == Guid.Empty)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }


            //Phone Number Validation and Verification
            if (!accountService.PhoneNumberIsValid(model.PhoneNumber))
            {
                ModelState.AddModelError("", "Invalid Phone Number");
                return RedirectToAction("Manage", "Account", new { message = $"Invalid phone number: {model.PhoneNumber}", classOfMessage = "text-bg-danger" });
            }
            if (!await accountService.PhoneNumberIsFree(model.PhoneNumber, userId))
            {
                ModelState.AddModelError("", "Phone Number is already taken");
                return RedirectToAction("Manage", "Account", new { message = $"Phone number is already taken: {model.PhoneNumber}", classOfMessage = "text-bg-danger" });
            }

            //Email Validation and Verification
            if (!accountService.EmailIsValid(model.Email))
            {
                ModelState.AddModelError("", "Invalid Email");
                return RedirectToAction("Manage", "Account", new { message = $"Invalid Email: {model.Email}", classOfMessage = "text-bg-danger" });
            }
            if (!await accountService.EmailIsFree(model.Email, userId))
            {
                ModelState.AddModelError("", "Email is already taken");
                return RedirectToAction("Manage", "Account", new { message = $"Email is already taken: {model.Email}", classOfMessage = "text-bg-danger" });
            }

            //Username Validation and Verification
            var username = $"{model.FirstName}.{model.LastName}".ToLower();
            if (!await accountService.UsernameIsFree(username, userId))
            {
                ModelState.AddModelError("", $"Username '{username}' is already taken");
                return RedirectToAction("Manage", "Account", new { message = $"Username '{username}' is already taken", classOfMessage = "text-bg-danger" });
            }

            try
            {
                await accountService.UpdateAsync(userId, model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Manage", "Account", new { message = "Something went wrong: " + ex.Message, classOfMessage = "text-bg-danger" });
            }

            return RedirectToAction("Manage", "Account", new { message = "Changes saved successfully", classOfMessage = "text-bg-success" });
        }

        [HttpGet]
        public IActionResult Delete()
        {
            var model = new UserDeleteViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteViewModel model)
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
                    return RedirectToAction("Manage", "Account", new { message = "Wrong Password", classOfMessage = "text-bg-danger" });
                }
                if (await roleService.UserIsInRoleAsync(userId.ToString(), "Principal"))
                {
                    return RedirectToAction("Manage", "School", new { message = "Delete your school first", classOfMessage = "text-bg-danger" });
                }

                await accountService.DeleteAsync(userId);
                //await userManager.DeleteAsync(user);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Manage", "Account", new { message = ex.Message, classOfMessage = "text-bg-danger" });
            }

            return RedirectToAction("Index", "Home", new { message = "You have successfully deleted your profile !", classOfMessage = "text-bg-success" });
        }

        [HttpGet]
        [Authorize(Policy = "IsPartOfSchoolButNotPrincipal")]
        public async Task<IActionResult> QuitSchool()
        {
            var userId = this.GetUserId();

            var school = await schoolService.GetSchoolByUserIdAsync(userId);
            if (school is null)
            {
                return RedirectToAction("Index", "Home", new { message = "You are not part of any school yet", classOfMessage = "text-bg-danger" });
            }

            var model = new UserQuitSchoolViewModel();
            ViewBag.SchoolName = school.Name;

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "IsPartOfSchoolButNotPrincipal")]
        public async Task<IActionResult> QuitSchool(UserQuitSchoolViewModel model)
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
                    return RedirectToAction("Manage", "Account", new { message = "Wrong Password", classOfMessage = "text-bg-danger" });
                }
                if (await roleService.UserIsInRoleAsync(userId.ToString(), "Principal"))
                {
                    return RedirectToAction("Manage", "School", new { message = "You cannot quit your school, you have to delete it", classOfMessage = "text-bg-danger" });
                }
                Guid schoolId;
                if (user.SchoolId is null || user.SchoolId == Guid.Empty)
                {
                    return RedirectToAction("Index", "Home", new { message = "You aren't a part of any school", classOfMessage = "text-bg-danger" });
                }
                schoolId = Guid.Parse(user.SchoolId.ToString()!);
                await schoolService.RemoveUserFromSchoolAsync(userId, schoolId);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Manage", "Account", new { message = ex.Message, classOfMessage = "text-bg-danger" });
            }

            return RedirectToAction("Index", "Home", new { message = "You have successfully unsigned from your school!", classOfMessage = "text-bg-success" });
        }

        //[HttpGet]
        //[Authorize(Policy = "Admin")]
        //public async Task<IActionResult> AdminPanel(string message = "", string classOfMessage = "")
        //{
        //    try
        //    {
        //        var userId = this.GetUserId();
        //        AdminPanelViewModel model = await accountService.GetAdminPanelViewModel(userId);
        //        ViewBag.Message = message;
        //        ViewBag.ClassOfMessage = classOfMessage;
        //        return View(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return RedirectToAction("Index", "Home", new { message = ex.Message, classOfMessage = "text-bg-danger" });
        //    }
        //}

        //[HttpGet]
        //[Authorize(Policy = "Admin")]
        //public async Task<IActionResult> DeleteUser(Guid id, string message = "", string classOfMessage = "")
        //{
        //    var userId = this.GetUserId();

        //    if (!await roleService.UserIsInRoleAsync(userId.ToString(), "Admin"))
        //    {
        //        return RedirectToAction("AccessDenied", new { msg = "You are not authorized to delete users" });
        //    }

        //    AdminUserDeletionViewModel model = await accountService.GetAdminUserDeletionViewModelAsync(id);

        //    ViewBag.Message = message;
        //    ViewBag.ClassOfMessage = classOfMessage;

        //    return View(model);
        //}

        //[HttpPost]
        //[Authorize(Policy = "Admin")]
        //public async Task<IActionResult> DeleteUser(AdminUserDeletionViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ModelState.AddModelError("", "Something went wrong!");
        //        return View(model);
        //    }


        //    try
        //    {
        //        var userId = GetUserId();
        //        var user = await userManager.FindByIdAsync(userId.ToString());
        //        if (user is null)
        //        {
        //            return RedirectToAction("Login", "Account");
        //        }
        //        var correctPassword = await userManager.CheckPasswordAsync(user, model.Password);
        //        if (!correctPassword)
        //        {
        //            return RedirectToAction("AdminPanel", "Account", new { message = "Wrong Password", classOfMessage = "text-bg-danger" });
        //        }
        //        if (await roleService.UserIsInRoleAsync(model.Id.ToString(), "Principal"))
        //        {
        //            return RedirectToAction("AdminPanel", "Account", new { message = "Delete school first", classOfMessage = "text-bg-danger" });
        //        }
        //        if (!await roleService.UserIsInRoleAsync(userId.ToString(), "Admin"))
        //        {
        //            return RedirectToAction("Index", "Home", new { message = "You are not authorized to delete accounts :) !", classOfMessage = "text-bg-danger" });
        //        }
        //        var userToDelete = await userManager.FindByIdAsync(model.Id.ToString());
        //        await userManager.DeleteAsync(userToDelete);
        //    }
        //    catch (Exception ex)
        //    {
        //        return RedirectToAction("AdminPanel", "Account", new { message = ex.Message, classOfMessage = "text-bg-danger" });
        //    }

        //    return RedirectToAction("AdminPanel", "Account", new { message = "Successfully deleted account !", classOfMessage = "text-bg-success" });
        //}

        //[HttpGet]
        //[Authorize(Policy = "Admin")]
        //public async Task<IActionResult> MakeAdmin(Guid id, string message = "", string classOfMessage = "")
        //{
        //    var userId = this.GetUserId();

        //    if (!await roleService.UserIsInRoleAsync(userId.ToString(), "Admin"))
        //    {
        //        return RedirectToAction("AccessDenied", new { msg = "You are not authorized to delete users" });
        //    }

        //    MakeUserAdminViewModel model = await accountService.GetMakeUserAdminViewModelAsync(id);

        //    ViewBag.Message = message;
        //    ViewBag.ClassOfMessage = classOfMessage;

        //    return View(model);
        //}

        //[HttpPost]
        //[Authorize(Policy = "Admin")]
        //public async Task<IActionResult> MakeAdmin(MakeUserAdminViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ModelState.AddModelError("", "Something went wrong!");
        //        return View(model);
        //    }


        //    try
        //    {
        //        var userId = GetUserId();
        //        var user = await userManager.FindByIdAsync(userId.ToString());
        //        if (user is null)
        //        {
        //            return RedirectToAction("Login", "Account");
        //        }
        //        var correctPassword = await userManager.CheckPasswordAsync(user, model.Password);
        //        if (!correctPassword)
        //        {
        //            return RedirectToAction("AdminPanel", "Account", new { message = "Wrong Password", classOfMessage = "text-bg-danger" });
        //        }
        //        if (await roleService.UserIsInRoleAsync(model.Id.ToString(), "Principal"))
        //        {
        //            return RedirectToAction("AdminPanel", "Account", new { message = "Delete school first", classOfMessage = "text-bg-danger" });
        //        }
        //        if (!await roleService.UserIsInRoleAsync(userId.ToString(), "Admin"))
        //        {
        //            return RedirectToAction("Index", "Home", new { message = "You are not authorized to give admin permissions :) !", classOfMessage = "text-bg-danger" });
        //        }
        //        var userToMakeAdmin = await userManager.FindByIdAsync(model.Id.ToString());
        //        await accountService.MakeAdmin(userToMakeAdmin);
        //        await signInManager.RefreshSignInAsync(user);
        //    }
        //    catch (Exception ex)
        //    {
        //        return RedirectToAction("AdminPanel", "Account", new { message = ex.Message, classOfMessage = "text-bg-danger" });
        //    }

        //    return RedirectToAction("AdminPanel", "Account", new { message = "Successfully given administrator permissions !", classOfMessage = "text-bg-success" });
        //}

    }
}
