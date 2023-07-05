using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Models;
using System.Diagnostics;

namespace SchoolSocialMediaApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> logger;
        private readonly IRoleService roleService;

        public HomeController(ILogger<HomeController> _logger, IRoleService _roleService)
        {
            this.logger = _logger;
            this.roleService = _roleService;
        }


        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
           // if (this.User?.Identity?.IsAuthenticated ?? false)
           // {
           //     return RedirectToAction("Feed", "Home");
           // }

           //if (!await roleService.RoleExistsAsync("Principal"))
           // {
           //     return Error();
           // }

            return View();
        }
        public IActionResult Feed()
        {
            return View();
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}