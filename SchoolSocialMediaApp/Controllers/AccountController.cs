using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.Models;
using SchoolSocialMediaApp.ViewModels.Models;

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
            //ModelState.AddModelError("", "The Password must contain: ");

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
            if(!await accountService.PhoneNumberIsFree(model.PhoneNumber))
            {
                ModelState.AddModelError("", "Phone Number is already taken");
                return View(model);
            }

            //Email Validation and Verification
            if(!accountService.EmailIsValid(model.Email))
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
        public IActionResult Login(string returnUrl)
        {
            var model = new LoginViewModel();
            model.ReturnUrl = returnUrl;

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "/")
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
            return Redirect(returnUrl ?? "/");
        }

        public async Task<IActionResult> Logout()
        {
            await accountService.LogoutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

    }
}
