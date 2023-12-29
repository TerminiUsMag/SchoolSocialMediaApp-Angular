using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.ViewModels.Models.SchoolSubject;

namespace SchoolSocialMediaApp.Controllers
{
    public class SchoolSubjectController : BaseController
    {
        private readonly ISchoolSubjectService schoolSubjectService;
        private readonly IRoleService roleService;
        private readonly ISchoolService schoolService;

        public SchoolSubjectController(ISchoolSubjectService _schoolSubjectService, IRoleService _roleService, ISchoolService _schoolService)
        {
            this.schoolSubjectService = _schoolSubjectService;
            this.roleService = _roleService;
            this.schoolService = _schoolService;

        }

        [HttpGet]
        public async Task<IActionResult> ManageAll(Guid schoolId, Guid userId, string message = "", string classOfMessage = "")
        {
            if (userId == Guid.Empty)
                userId = this.GetUserId();
            var userIdString = userId.ToString();
            var isPrincipal = await schoolService.IsTheUserPrincipalOfTheSchool(schoolId, userId);
            var isAdmin = await roleService.UserIsInRoleAsync(userIdString, "Admin");
            if (isPrincipal || isAdmin)
            {
                var subjectsInSchool = await schoolSubjectService.GetAllSubjectsInSchoolAsync(schoolId, userId);

                ViewBag.Message = message;
                ViewBag.ClassOfMessage = classOfMessage;
                ViewBag.SchoolId = schoolId;

                return View(subjectsInSchool);
            }
            else
                return RedirectToAction(nameof(HomeController.Index), new { message = "You don't have permission to Manage subjects", classOfMessage = "text-bg-danger" });

        }

        [HttpGet]
        public async Task<IActionResult> Manage(Guid schoolId, Guid userId, Guid subjectId, string message = "", string classOfMessage = "")
        {
            if (userId == Guid.Empty)
                userId = this.GetUserId();
            var userIdString = userId.ToString();
            var isPrincipal = await schoolService.IsTheUserPrincipalOfTheSchool(schoolId, userId);
            var isAdmin = await roleService.UserIsInRoleAsync(userIdString, "Admin");
            if (isPrincipal || isAdmin)
            {
                var model = await schoolSubjectService.GetSubjectById(subjectId);

                ViewBag.Message = message;
                ViewBag.ClassOfMessage = classOfMessage;

                return View(model);
            }
            else
                return RedirectToAction(nameof(HomeController.Index), new { message = "You don't have permission to Manage a subject", classOfMessage = "text-bg-danger" });
        }

        [HttpGet]
        public async Task<IActionResult> AssignToNewClass(Guid schoolId, Guid userId, Guid subjectId, string subjectName, string message = "", string classOfMessage = "")
        {
            if (userId == Guid.Empty)
                userId = this.GetUserId();
            var userIdString = userId.ToString();
            var isPrincipal = await schoolService.IsTheUserPrincipalOfTheSchool(schoolId, userId);
            var isAdmin = await roleService.UserIsInRoleAsync(userIdString, "Admin");
            if (isPrincipal || isAdmin)
            {
                var classes = await schoolSubjectService.GetAllAssignableToSubjectClassesAsync(schoolId, subjectId);
                ViewBag.Message = message;
                ViewBag.ClassOfMessage = classOfMessage;
                ViewBag.SubjectName = subjectName;
                ViewBag.SubjectId = subjectId;
                //ViewBag.SchoolId = schoolId;

                return View(classes);
            }
            else
                return RedirectToAction(nameof(HomeController.Index), new { message = "You don't have permission to Assign classes to subjects", classOfMessage = "text-bg-danger" });
        }

        [HttpPost]
        public async Task<IActionResult> AddClassToSubject(Guid classId, Guid subjectId, Guid schoolId, Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                    userId = this.GetUserId();
                var userIdString = userId.ToString();
                var isPrincipal = await schoolService.IsTheUserPrincipalOfTheSchool(schoolId, userId);
                var isAdmin = await roleService.UserIsInRoleAsync(userIdString, "Admin");
                if (isPrincipal || isAdmin)
                {
                    await schoolSubjectService.AssignClassToSubject(schoolId, classId, subjectId);
                    return RedirectToAction(nameof(SchoolController.Manage), new { schoolId = schoolId, subjectId = subjectId, message = "Class assigned to subject successfully!", classOfMessage = "text-bg-success" });
                }
                else
                    throw new ArgumentException("You don't have permission to Assign classes to subjects in this school");
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(HomeController.Index), new { message = ex.Message, classOfMessage = "text-bg-danger" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UnAssignClassFromSubject(Guid classId, Guid subjectId, Guid schoolId, Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                    userId = this.GetUserId();
                var userIdString = userId.ToString();
                var isPrincipal = await schoolService.IsTheUserPrincipalOfTheSchool(schoolId, userId);
                var isAdmin = await roleService.UserIsInRoleAsync(userIdString, "Admin");
                if (isPrincipal || isAdmin)
                {
                    await schoolSubjectService.UnAssignClassFromSubject(schoolId, classId, subjectId);
                    return RedirectToAction(nameof(SchoolController.Manage), new { schoolId = schoolId, subjectId = subjectId, message = "Class unassigned from subject successfully!", classOfMessage = "text-bg-success" });
                }
                else
                    throw new ArgumentException("You don't have permission to Unassign classes from subjects in this school");
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(HomeController.Index), new { message = ex.Message, classOfMessage = "text-bg-danger" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid schoolId)
        {
            var model = new SchoolSubjectCreateModel();
            try
            {
                model.SchoolId = schoolId;
                var userId = this.GetUserId();
                var userIdString = userId.ToString();
                var isPrincipal = await schoolService.IsTheUserPrincipalOfTheSchool(model.SchoolId, userId);
                var isAdmin = await roleService.UserIsInRoleAsync(userIdString, "Admin");
                if (isPrincipal)
                    model.CandidateTeachers = await schoolSubjectService.GetCandidateTeachersInSchool(model.SchoolId, userId);
                else if (isAdmin)
                    model.CandidateTeachers = await schoolSubjectService.GetCandidateTeachersInSchool(model.SchoolId, userId, true);
                else
                    throw new ArgumentException("You don't have permission to create subjects in this school");
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(HomeController.Index), new { message = ex.Message, classOfMessage = "text-bg-danger" });
            }
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Create(SchoolSubjectCreateModel model)
        {
            var userId = this.GetUserId();
            if (!ModelState.IsValid)
            {
                var userIdString = userId.ToString();
                var isPrincipal = await schoolService.IsTheUserPrincipalOfTheSchool(model.SchoolId, userId);
                var isAdmin = await roleService.UserIsInRoleAsync(userIdString, "Admin");
                if (isPrincipal)
                    model.CandidateTeachers = await schoolSubjectService.GetCandidateTeachersInSchool(model.SchoolId, userId);
                else if (isAdmin)
                    model.CandidateTeachers = await schoolSubjectService.GetCandidateTeachersInSchool(model.SchoolId, userId, true);
                else
                    throw new ArgumentException("You don't have permission to create subjects in this school");

                return View(model);
            }

            try
            {
                var subjectCreated = await schoolSubjectService.CreateSchoolSubjectAsync(model, userId);
                if (!subjectCreated)
                {
                    throw new ArgumentException("Something went wrong when creating a new school subject");
                }
            }
            catch (ArgumentException ae)
            {
                ModelState.AddModelError("", ae.Message);
                model.CandidateTeachers = await schoolSubjectService.GetCandidateTeachersInSchool(model.SchoolId, userId);
                return View(model);
            }
            var schoolId = await schoolService.GetSchoolIdByUserIdAsync(userId);

            return RedirectToAction(nameof(ManageAll), new { schoolId = schoolId, userId = userId, message = "Subject created successfully", classOfMessage = "text-bg-success" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid schoolId, Guid subjectId, Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                    userId = this.GetUserId();
                var userIdString = userId.ToString();
                var isPrincipal = await schoolService.IsTheUserPrincipalOfTheSchool(schoolId, userId);
                var isAdmin = await roleService.UserIsInRoleAsync(userIdString, "Admin");
                if (isPrincipal || isAdmin)
                {
                    await schoolSubjectService.DeleteSubject(userId, subjectId);
                    return RedirectToAction(nameof(ManageAll), new { schoolId = schoolId, message = "Subject deleted successfully", classOfMessage = "text-bg-success" });
                }
                else
                    throw new ArgumentException("You don't have permission to delete subjects from this school");
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(HomeController.Index),"Home", new { message = ex.Message, classOfMessage = "text-bg-danger" });
            }
        }
    }
}
