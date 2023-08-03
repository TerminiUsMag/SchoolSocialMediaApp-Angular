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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAccountService accountService;
        private readonly IRoleService roleService;
        private readonly ISchoolService schoolService;

        public AccountController(IAccountService _accountService, UserManager<ApplicationUser> _userManager, IRoleService _roleService, ISchoolService _schoolService)
        {
            this.accountService = _accountService;
            this.userManager = _userManager;
            this.roleService = _roleService;
            this.schoolService = _schoolService;
        }
        //private readonly UserManager<ApplicationUser> userManager;
        //private readonly SignInManager<ApplicationUser> signInManager;

        //public AccountController(
        //    UserManager<ApplicationUser> _userManager,
        //    SignInManager<ApplicationUser> _signInManager)
        //{
        //    this.userManager = _userManager;
        //    this.signInManager = _signInManager;
        //}

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            var model = new RegisterViewModel();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            //Username Validation and Verification
            var username = $"{model.FirstName}.{model.LastName}".ToLower();

            if (!await accountService.UsernameIsFree(username))
            {
                ModelState.AddModelError("", $"Username '{username}' is already taken");
                return View(model);
            }

            //Phone Number Validation and Verification
            if (!accountService.PhoneNumberIsValid(model.PhoneNumber))
            {
                ModelState.AddModelError("", "Invalid Phone Number");
                return View(model);
            }
            if (!await accountService.PhoneNumberIsFree(model.PhoneNumber))
            {
                ModelState.AddModelError("", "Phone Number is already taken");
                return View(model);
            }

            //Email Validation and Verification
            if (!accountService.EmailIsValid(model.Email))
            {
                ModelState.AddModelError("", "Invalid Email");
                return View(model);
            }
            if (!await accountService.EmailIsFree(model.Email))
            {
                ModelState.AddModelError("", "Email is already taken");
                return View(model);
            }

            //Model Validation
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
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
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "/")
        {
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
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
            }

            var result = await accountService.LoginAsync(model.Email, model.Password, model.RememberMe = false);

            if (!result)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
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
        public IActionResult BecomePrincipal(string errorMsg)
        {
            ViewBag.ErrorMsg = errorMsg;
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "CanBePrincipal")]
        public IActionResult PrincipalCreate()
        {
            var userId = this.GetUserId();
            if (userId != Guid.Empty)
            {
                return RedirectToAction(nameof(SchoolController.Register), "School");
            }

            return RedirectToAction(nameof(BecomePrincipal), new { errorMsg = "Something went wrong" });
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
            var userId = this.GetUserId();
            if (userId == Guid.Empty)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }


            //Phone Number Validation and Verification
            if (!accountService.PhoneNumberIsValid(model.PhoneNumber))
            {
                ModelState.AddModelError("", "Invalid Phone Number");
                return View(model);
            }
            if (!await accountService.PhoneNumberIsFree(model.PhoneNumber))
            {
                ModelState.AddModelError("", "Phone Number is already taken");
                return View(model);
            }

            //Email Validation and Verification
            if (!accountService.EmailIsValid(model.Email))
            {
                ModelState.AddModelError("", "Invalid Email");
                return View(model);
            }
            if (!await accountService.EmailIsFree(model.Email))
            {
                ModelState.AddModelError("", "Email is already taken");
                return View(model);
            }

            //Username Validation and Verification
            var username = $"{model.FirstName}.{model.LastName}".ToLower();
            if (!await accountService.UsernameIsFree(username))
            {
                ModelState.AddModelError("", $"Username '{username}' is already taken");
                return View(model);
            }

            //Model Validation
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
            }

            try
            {
                await accountService.UpdateAsync(userId, model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
            }

            return RedirectToAction("AccountChangesSuccess");
        }

        [HttpGet]
        public IActionResult AccountChangesSuccess()
        {
            return View();
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
                    return RedirectToAction("Manage", "Account", new { message = "Wrong Password", classOfMessage = "text-bg-danger" });
                }
                if (await roleService.UserIsInRoleAsync(userId.ToString(), "Principal"))
                {
                    return RedirectToAction("Manage", "School", new { message = "Delete your school first", classOfMessage = "text-bg-danger" });
                }

                await userManager.DeleteAsync(user);
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

        public async Task<IActionResult> AdminPanel()
        {
            return View();
        }

    }
}
