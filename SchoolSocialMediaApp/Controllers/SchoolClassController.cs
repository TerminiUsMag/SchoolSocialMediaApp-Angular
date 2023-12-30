﻿using Microsoft.AspNetCore.Mvc;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.SchoolClass;

namespace SchoolSocialMediaApp.Controllers
{
    public class SchoolClassController : BaseController
    {
        private readonly ISchoolClassService schoolClassService;
        private readonly IRoleService roleService;

        public SchoolClassController(ISchoolClassService _schoolClassService, IRoleService _roleService)
        {
            this.schoolClassService = _schoolClassService;
            this.roleService = _roleService;
        }
        [HttpGet]
        public async Task<IActionResult> ManageAll(Guid schoolId, Guid userId, string message = "", string classOfMessage = "")
        {
            ICollection<SchoolClassViewModel>? classes;
            if (userId == Guid.Empty)
                userId = this.GetUserId();
            try
            {
                classes = await schoolClassService.GetAllClassesAsync(schoolId, userId);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(HomeController.Index), new { message = ex.Message, classOfMessage = "text-bg-danger" });
            }

            ViewBag.Message = message;
            ViewBag.ClassOfMessage = classOfMessage;

            return View(classes);
        }
        [HttpGet]
        public async Task<IActionResult> Manage(Guid classId, Guid schoolId, string message = "", string classOfMessage = "")
        {
            var schoolClass = await schoolClassService.GetClassByIdAsync(classId, this.GetUserId());

            ViewBag.Message = message;
            ViewBag.ClassOfMessage = classOfMessage;

            return View(schoolClass);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new SchoolClassCreateModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SchoolClassCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = this.GetUserId();
            try
            {
                await schoolClassService.CreateSchoolClassAsync(model, userId);
            }
            catch (ArgumentException ae)
            {
                ModelState.AddModelError("", ae.Message);
                return View(model);
            }

            return RedirectToAction(nameof(ManageAll), new { schoolId = Guid.Empty, userId = userId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid classId)
        {
            try
            {
                var userId = this.GetUserId();
                var isPrincipal = await roleService.UserIsInRoleAsync(userId.ToString(), "Principal");
                var isAdmin = await roleService.UserIsInRoleAsync(userId.ToString(), "Admin");
                if (!isPrincipal && !isAdmin)
                {
                    throw new ArgumentException("You don't have permission to Delete School Classes!");
                }
                await schoolClassService.RemoveAllStudentsFromClassAsync(classId);
                await schoolClassService.RemoveAllSubjectsFromClassAsync(classId);
                //await schoolClassService.DeleteClassAsync(classId);
                return RedirectToAction(nameof(ManageAll), new { userId = this.GetUserId(), message = "Class deleted successfully", classOfMessage = "text-bg-success" });
            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    return RedirectToAction(nameof(ManageAll), new { userId = this.GetUserId(), message = ex.Message, classOfMessage = "text-bg-danger" });
                }

                return RedirectToAction(nameof(ManageAll), new { userId = this.GetUserId(), message = "An error occurred while deleting class", classOfMessage = "text-bg-danger" });
            }

        }

        [HttpGet]
        public async Task<IActionResult> AddStudentsToClass(Guid schoolId, Guid classId, string message = "", string classOfMessage = "")
        {
            var students = new List<ApplicationUser>();
            students = await schoolClassService.GetAllFreeStudentsAsync(schoolId);

            ViewBag.Message = message;
            ViewBag.ClassOfMessage = classOfMessage;
            ViewBag.ClassId = classId;

            return View(students);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudentToClass(Guid studentId, Guid classId, Guid schoolId)
        {
            try
            {
                var userId = this.GetUserId();
                var isPrincipal = await roleService.UserIsInRoleAsync(userId.ToString(), "Principal");
                var isAdmin = await roleService.UserIsInRoleAsync(userId.ToString(), "Admin");
                if (!isPrincipal && !isAdmin)
                {
                    throw new ArgumentException("You don't have permission to Add student to Class");
                }
                await schoolClassService.AddStudentToClassAsync(studentId, classId);
                return RedirectToAction(nameof(AddStudentsToClass), new { classId = classId, schoolId = schoolId, message = "Student added successfully!", classOfMessage = "text-bg-success" });
            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    return RedirectToAction(nameof(AddStudentsToClass), new { classId = classId, schoolId = schoolId, message = ex.Message, classOfMessage = "text-bg-danger" });
                }
                return RedirectToAction(nameof(AddStudentsToClass), new { classId = classId, schoolId = schoolId, message = "An error occurred while trying to add the student to the class", classOfMessage = "text-bg-danger" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveStudentFromClass(Guid studentId, Guid classId, Guid schoolId)
        {
            try
            {
                var userId = this.GetUserId();
                var isPrincipal = await roleService.UserIsInRoleAsync(userId.ToString(), "Principal");
                var isAdmin = await roleService.UserIsInRoleAsync(userId.ToString(), "Admin");
                if (!isPrincipal && !isAdmin)
                {
                    throw new ArgumentException("You don't have permission to Remove student from Class");
                }
                await schoolClassService.RemoveStudentFromClassAsync(studentId, classId);
                return RedirectToAction(nameof(Manage), new { classId = classId, schoolId = schoolId, message = "Student removed from class successfully!", classOfMessage = "text-bg-success" });

            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    return RedirectToAction(nameof(Manage), new { classId = classId, schoolId = schoolId, message = ex.Message, classOfMessage = "text-bg-danger" });
                }

                return RedirectToAction(nameof(Manage), new { classId = classId, schoolId = schoolId, message = "An error occurred while trying to remove student from class", classOfMessage = "text-bg-danger" });
            }

        }
    }
}
