using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.User;

namespace SchoolSocialMediaApp.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService _accountService)
        {
            this.accountService = _accountService;
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

        [AllowAnonymous]
        [HttpPost]
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
        public async Task<IActionResult> Manage()
        {
            var userId = this.GetUserId();
            if (userId == Guid.Empty)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var model = await accountService.GetUserManageViewModelAsync(userId.ToString());

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

    }
}
