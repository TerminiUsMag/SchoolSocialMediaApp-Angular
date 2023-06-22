using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.Models;

namespace SchoolSocialMediaApp.Controllers
{
    public class AccountController : Controller
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
        public IActionResult Register()
        {
            var model = new RegisterViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = $"{model.FirstName}.{model.LastName}",
                PhoneNumber = model.PhoneNumber,
                CreatedOn = DateTime.Now,

            };
            if (!await accountService.RegisterAsync(user, model.Password))
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
            }
            

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
