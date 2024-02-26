using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Controllers;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.Student;

namespace SchoolSocialMediaApp.Areas.Student.Controllers
{
    public class StudentController : BaseController
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAccountService accountService;
        private readonly IRoleService roleService;
        private readonly ISchoolService schoolService;
        private readonly IPostService postService;
        private readonly ISchoolSubjectService subjectService;

        public StudentController(SignInManager<ApplicationUser> _signInManager,
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
        [Area("Student")]
        [Authorize(Policy = "Student")]
        public async Task<IActionResult> StudentPanel(string message = "", string classOfMessage = "")
        {
            try
            {
                var userId = this.GetUserId();

                var isTeacher = await roleService.UserIsInRoleAsync(userId.ToString(), "Student");
                if (!isTeacher)
                    throw new ArgumentException("You are not a student and don't have access to the Student Panel.");

                StudentPanelViewModel model = await accountService.GetStudentPanelViewModel(userId);
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
