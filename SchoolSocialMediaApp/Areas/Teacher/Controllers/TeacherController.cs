using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Controllers;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.Admin;

namespace SchoolSocialMediaApp.Areas.Teacher.Controllers
{
    public class TeacherController : BaseController
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAccountService accountService;
        private readonly IRoleService roleService;
        private readonly ISchoolService schoolService;
        private readonly IPostService postService;

        public TeacherController(SignInManager<ApplicationUser> _signInManager, UserManager<ApplicationUser> _userManager, IRoleService _roleService, ISchoolService _schoolService, IAccountService _accountService, IPostService _postService)
        {
            this.accountService = _accountService;
            this.userManager = _userManager;
            this.roleService = _roleService;
            this.schoolService = _schoolService;
            this.signInManager = _signInManager;
            this.postService = _postService;
        }


        [HttpGet]
        [Area("Teacher")]
        [Authorize(Policy = "Teacher")]
        public async Task<IActionResult> TeacherPanel(string message = "", string classOfMessage = "")
        {
            try
            {
                var userId = this.GetUserId();
                var model = await accountService.GetTeacherPanelViewModel(userId);
                ViewBag.Message = message;
                ViewBag.ClassOfMessage = classOfMessage;
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home", new { message = ex.Message, classOfMessage = "text-bg-danger" });
            }
        }
    }
}
