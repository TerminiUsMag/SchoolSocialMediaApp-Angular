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

        public async Task<bool> CreateSchoolClassAsync(SchoolClassCreateModel schoolClassCreateModel, Guid userId)
        {
            try
            {
                Guid schoolId;

                var school = await schoolService.GetSchoolByUserIdAsync(userId);
                schoolId = school.Id;
                var duplicate = await repo
                    .AllReadonly<SchoolClass>()
                    .AnyAsync(sc => sc.Name == schoolClassCreateModel.Name && sc.Grade == schoolClassCreateModel.Grade);
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

        public async Task<bool> DeleteClass(Guid classId, Guid userId)
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
                    if (schoolClass is not null)
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

        public async Task<SchoolClassViewModel> GetClassByIdAsync(Guid classId)
        {
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
    }
}
