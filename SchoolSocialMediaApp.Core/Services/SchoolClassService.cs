using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Common;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.Infrastructure.Migrations;
using SchoolSocialMediaApp.ViewModels.Models.SchoolClass;

namespace SchoolSocialMediaApp.Core.Services
{
    public class SchoolClassService : ISchoolClassService
    {
        private readonly IRepository repo;
        private readonly ISchoolService schoolService;
        private readonly IRoleService roleService;

        public SchoolClassService(IRepository _repo, ISchoolService _schoolService, IRoleService _roleService)
        {
            this.repo = _repo;
            this.schoolService = _schoolService;
            this.roleService = _roleService;
        }

        public async Task AddStudentToClassAsync(Guid studentId, Guid classId)
        {
            var schoolClass = await repo.All<SchoolClass>().FirstOrDefaultAsync(sc => sc.Id == classId);

            if (schoolClass == null)
            {
                throw new ArgumentException("No school with this ID");
            }

            var student = await repo.All<ApplicationUser>().FirstOrDefaultAsync(s => s.Id == studentId);
            if (student == null)
            {
                throw new ArgumentException("No user with this ID");
            }

            schoolClass.Students.Add(student);
            student.ClassId = classId;
            await repo.SaveChangesAsync();
        }

        public async Task<bool> CreateSchoolClassAsync(SchoolClassCreateModel schoolClassCreateModel, Guid userId)
        {
            try
            {
                Guid schoolId;
                var isAdmin = await roleService.UserIsInRoleAsync(userId.ToString(), "Admin");
                var isPrincipal = await roleService.UserIsInRoleAsync(userId.ToString(), "Principal");
                if (!isPrincipal && !isAdmin)
                    return false;

                var school = await schoolService.GetSchoolByUserIdAsync(userId);
                schoolId = school.Id;
                var duplicate = await repo
                    .AllReadonly<SchoolClass>()
                    .AnyAsync(sc => sc.Name == schoolClassCreateModel.Name && sc.Grade == schoolClassCreateModel.Grade && sc.SchoolId == schoolId);
                if (duplicate)
                {
                    throw new ArgumentException("The class you're trying to create already exists !");
                }

                var schoolClass = new SchoolClass
                {
                    Id = new Guid(),
                    CreatedOn = DateTime.Now,
                    Grade = schoolClassCreateModel.Grade,
                    Name = schoolClassCreateModel.Name,
                    SchoolId = schoolId,
                    Students = new List<ApplicationUser>(),
                    Subjects = new List<SchoolSubject>(),
                };
                await repo.AddAsync(schoolClass);
                await repo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.Message == "The class you're trying to create already exists !")
                {
                    throw;
                }
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteClassAsync(Guid classId, Guid userId)
        {
            bool userHasPermission = false;

            var isPrincipal = await roleService.UserIsInRoleAsync(userId.ToString(), "Principal");
            var isAdmin = await roleService.UserIsInRoleAsync(userId.ToString(), "Admin");

            if (isPrincipal || isAdmin)
            {
                userHasPermission = true;
            }

            if (userHasPermission)
            {
                var schoolClass = await repo.All<SchoolClass>().FirstOrDefaultAsync(sc => sc.Id == classId);
                try
                {
                    if (schoolClass is not null && !schoolClass.Students.Any() && !schoolClass.Subjects.Any())
                    {
                        repo.Delete(schoolClass);
                        await repo.SaveChangesAsync();
                        return true;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return false;
        }

        public async Task<ICollection<SchoolClassViewModel>> GetAllClassesAsync(Guid schoolId, Guid userId)
        {
            if (userId == Guid.Empty && schoolId == Guid.Empty)
            {
                throw new ArgumentException("Both arguments are empty!");
            }
            else if (schoolId == Guid.Empty)
            {
                schoolId = (await schoolService.GetSchoolByUserIdAsync(userId)).Id;
            }
            bool hasPermission = false;

            var isPrincipal = await roleService.UserIsInRoleAsync(userId.ToString(), "Principal");
            var isAdmin = await roleService.UserIsInRoleAsync(userId.ToString(), "Admin");

            if (isPrincipal || isAdmin)
            {
                hasPermission = true;
            }

            if (!hasPermission)
            {
                throw new ArgumentException("User doesn't have the required permissions! ");
            }

            var classes = await repo
                 .All<SchoolClass>()
                 .Where(sc => sc.SchoolId == schoolId)
                 .OrderBy(sc => sc.Grade)
                 .Select(sc => new SchoolClassViewModel
                 {
                     Id = sc.Id,
                     SchoolId = sc.SchoolId,
                     Grade = sc.Grade,
                     Name = sc.Name,
                     Students = sc.Students,
                     Subjects = sc.Subjects,
                     CreatedOn = sc.CreatedOn,
                 }).ToListAsync();

            return classes;
        }

        public async Task<List<ApplicationUser>> GetAllFreeStudentsAsync(Guid schoolId)
        {
            var usersInSchool = await schoolService.GetAllUsersInSchool(schoolId);
            var students = new List<ApplicationUser>();

            foreach (var user in usersInSchool)
            {
                if (await roleService.UserIsInRoleAsync(user.Id.ToString(), "Student") && user.ClassId is null)
                {
                    students.Add(user);
                }
            }
            return students;
        }

        public async Task<SchoolClassViewModel> GetClassByIdAsync(Guid classId, Guid userId)
        {
            bool hasPermission = false;

            var isPrincipal = await roleService.UserIsInRoleAsync(userId.ToString(), "Principal");
            var isAdmin = await roleService.UserIsInRoleAsync(userId.ToString(), "Admin");

            if (isPrincipal || isAdmin)
            {
                hasPermission = true;
            }

            if (!hasPermission)
            {
                throw new ArgumentException("User doesn't have the required permissions! ");
            }

            var schoolClass = await repo
                .All<SchoolClass>()
                .Where(sc => sc.Id == classId)
                .Select(sc => new SchoolClassViewModel
                {
                    Id = sc.Id,
                    SchoolId = sc.SchoolId,
                    Grade = sc.Grade,
                    Name = sc.Name,
                    CreatedOn = sc.CreatedOn,
                    Students = sc.Students,
                    Subjects = sc.Subjects,
                })
                .FirstOrDefaultAsync();

            if (schoolClass is null)
            {
                throw new ArgumentException("School class with this Id does not exist ( " + classId + " )");
            }

            return schoolClass;
        }

        public async Task RemoveAllStudentsFromClassAsync(Guid classId)
        {
            var schoolClass = await repo.All<SchoolClass>().Include(sc=>sc.Students).FirstOrDefaultAsync(sc => sc.Id == classId);
            if (schoolClass is null)
            {
                throw new ArgumentException("No such class");
            }
            foreach (var student in schoolClass.Students)
            {
                student.ClassId = null;
            }
            schoolClass.Students.Clear();
            await repo.SaveChangesAsync();
        }

        public async Task RemoveStudentFromClassAsync(Guid studentId, Guid classId)
        {
            var student = await repo.All<ApplicationUser>().FirstOrDefaultAsync(s => s.Id == studentId);
            if (student is null)
            {
                throw new ArgumentException("No such student");
            }
            var schoolClass = await repo.All<SchoolClass>().FirstOrDefaultAsync(sc => sc.Id == classId);
            if (schoolClass is null)
            {
                throw new ArgumentException("No such class");
            }
            if (student.ClassId == classId && schoolClass.Students.Any(s => s.Id == studentId))
            {
                schoolClass.Students.Remove(student);
                student.ClassId = null;
                await repo.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("The user is not part of this class");
            }
        }
    }
}
