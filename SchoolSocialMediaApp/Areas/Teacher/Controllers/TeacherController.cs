using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Controllers;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.SchoolSubject;

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
        private readonly ISchoolSubjectService subjectService;

        public TeacherController(SignInManager<ApplicationUser> _signInManager,
                                 UserManager<ApplicationUser> _userManager,
                                 IRoleService _roleService,
                                 ISchoolService _schoolService,
                                 IAccountService _accountService,
                                 IPostService _postService,
                                 ISchoolSubjectService _subjectService)
        {
            this.accountService = _accountService;
            this.userManager = _userManager;
            this.roleService = _roleService;
            this.schoolService = _schoolService;
            this.signInManager = _signInManager;
            this.postService = _postService;
            this.subjectService = _subjectService;
        }


        [HttpGet]
        [Area("Teacher")]
        [Authorize(Policy = "Teacher")]
        public async Task<IActionResult> TeacherPanel(string message = "", string classOfMessage = "")
        {
            try
            {
                var userId = this.GetUserId();

                var isTeacher = await roleService.UserIsInRoleAsync(userId.ToString(), "Teacher");
                if (!isTeacher)
                    throw new ArgumentException("You are not a teacher and don't have access to the Teacher Panel.");

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

        [HttpGet]
        [Area("Teacher")]
        [Authorize(Policy = "Teacher")]
        public async Task<IActionResult> ManageSubject(Guid subjectId)
        {
            try
            {
                var userId = this.GetUserId();
                var isTeacher = await roleService.UserIsInRoleAsync(userId.ToString(), "Teacher");
                if (!isTeacher)
                    throw new ArgumentException("You don't have permission to do this.");

                SchoolSubjectTeacherPanelViewModel model = await subjectService.GetSubjectForTeacherPanelByIdAsync(subjectId);

                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home", new { message = ex.Message, classOfMessage = "text-bg-danger" });
            }
        }

        [HttpGet]
        [Area("Teacher")]
        [Authorize(Policy = "Teacher")]
        public async Task<IActionResult> ManageClassInSubject(Guid classId, Guid subjectId)
        {
            try
            {
                var userId = this.GetUserId();
                var isTeacher = await roleService.UserIsInRoleAsync(userId.ToString(), "Teacher");

                if (!isTeacher)
                    throw new ArgumentException("You don't have permission to do this");



            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home", new { message = ex.Message, classOfMessage = "text-bg-danger" });
            }

            return View();
        }
    }
}
