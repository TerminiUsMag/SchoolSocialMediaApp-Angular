using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Common;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models;

namespace SchoolSocialMediaApp.Core.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly IRepository repo;
        private readonly IRoleService roleService;
        private readonly UserManager<ApplicationUser> userManager;

        public SchoolService(IRepository _repo, IRoleService _roleService, UserManager<ApplicationUser> _userManager)
        {
            this.repo = _repo;
            this.roleService = _roleService;
            this.userManager = _userManager;
        }

        public async Task<SchoolViewModel> CreateSchoolAsync(SchoolViewModel model, Guid userId)
        {
            //var principal = await repo.AllReadonly<Principal>().FirstOrDefaultAsync(x => x.UserId == userId);
            //if (principal is not null)
            //{
            //    throw new ArgumentException("User is already a principal of another school.");
            //}
            //principal = new Principal
            //{
            //    UserId = userId,
            //    Id = Guid.NewGuid()
            //};
            var userIsPrincipal = await roleService.UserIsInRoleAsync(userId.ToString(), "Principal");
            if (userIsPrincipal)
            {
                throw new ArgumentException("User is already a principal of another school.");
            }

            var userAddedToRole = await roleService.AddUserToRoleAsync(userId.ToString(), "Principal");
            if (!userAddedToRole)
            {
                throw new ArgumentException("User could not be added to role.");
            }

            var user = repo.All<ApplicationUser>().Where(x => x.Id == userId).FirstOrDefault();


            var school = new School
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl!,
                Location = model.Location,
                Principal = user!,
                PrincipalId = userId,
            };

            //principal.School = school;
            //principal.SchoolId = school.Id;

            //await repo.AddAsync<Principal>(principal);
            await repo.AddAsync<School>(school);

            await repo.SaveChangesAsync();

            return new SchoolViewModel
            {
                Id = school.Id,
                Name = school.Name,
                Description = school.Description,
                ImageUrl = school.ImageUrl,
                Location = school.Location,
                PrincipalId = school.PrincipalId,
                PrincipalName = user!.FirstName + " " + user.LastName
            };
        }

        public async Task DeleteSchoolAsync(Guid id)
        {
            var school = await repo.GetByIdAsync<School>(id);
            if (school is null)
            {
                throw new ArgumentException("School does not exist.");
            }

            repo.Delete<School>(school);

            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<SchoolViewModel>> GetAllSchoolsAsync()
        {
            if (await repo.All<School>().CountAsync() == 0)
            {
                throw new ArgumentException("There are no schools.");
            }
            var schools = await repo.All<School>().Select(s => new SchoolViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                ImageUrl = s.ImageUrl,
                Location = s.Location,
                PrincipalId = s.PrincipalId,
                PrincipalName = s.Principal.FirstName + " " + s.Principal./*User.*/LastName
            }).ToListAsync();

            return schools;
        }

        public async Task<SchoolViewModel> GetSchoolByIdAsync(Guid id)
        {
            var school = await repo.GetByIdAsync<School>(id);
            return new SchoolViewModel
            {
                Id = school.Id,
                Name = school.Name,
                Description = school.Description,
                ImageUrl = school.ImageUrl,
                Location = school.Location,
                PrincipalId = school.PrincipalId
            };
        }

        public async Task<SchoolViewModel> GetSchoolByUserIdAsync(Guid userId)
        {
            var school = await repo.AllReadonly<School>().Where(s => s.PrincipalId == userId).FirstOrDefaultAsync();
            if (school is null)
            {
                throw new ArgumentException("School does not exist.");
            }

            return new SchoolViewModel
            {
                Id = school.Id,
                Description = school.Description,
                ImageUrl = school.ImageUrl,
                Location = school.Location,
                Name = school.Name,
                PrincipalId = school.PrincipalId,
                PrincipalName = school.Principal.FirstName + " " + school.Principal.LastName
            };
        }

        public async Task<SchoolViewModel> GetSchoolIdByNameAsync(string name)
        {
            var school = await repo.All<School>().FirstOrDefaultAsync(s => s.Name == name);
            if (school is null)
            {
                throw new ArgumentException("School does not exist.");
            }
            return new SchoolViewModel
            {
                Id = school.Id,
                Name = school.Name,
                Description = school.Description,
                ImageUrl = school.ImageUrl,
                Location = school.Location,
                PrincipalId = school.PrincipalId
            };
        }


        public async Task<Guid> GetSchoolIdByUserIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User id cannot be empty.");
            }

            var roles = new List<string>() { "Principal", "Teacher", "Parent", "Student" };

            foreach (var role in roles)
            {
                if (await roleService.UserIsInRoleAsync(userId.ToString(), role))
                {
                    var user = await userManager.FindByIdAsync(userId.ToString());
                    var userEager = await repo.All<ApplicationUser>().Include(u => u.School).FirstOrDefaultAsync(u => u.Id == userId);
                    var schoolId = user.SchoolId;
                    if (schoolId == Guid.Empty || schoolId is null)
                    {
                        throw new ArgumentException("School id cannot be empty.");
                    }
                    return schoolId.Value;
                }
            }
            throw new ArgumentException("User is not a member of any school.");

            //var schoolId = await repo.AllReadonly<School>().Where(s => s.PrincipalId == userId).Select(s => s.Id).FirstOrDefaultAsync();

            //if (await repo.AllReadonly<Student>().AnyAsync(s => s.UserId == userId))//User is student
            //{
            //    school = await repo.AllReadonly<Student>().Where(s => s.UserId == userId).Select(s => s.School.Id).FirstOrDefaultAsync();
            //}
            //else if (await repo.AllReadonly<Parent>().AnyAsync(p => p.UserId == userId))//User is parent
            //{
            //    school = await repo.AllReadonly<Parent>().Where(p => p.UserId == userId).Select(p => p.School.Id).FirstOrDefaultAsync();
            //}
            //else if (await repo.AllReadonly<Teacher>().AnyAsync(t => t.UserId == userId))//User is teacher
            //{
            //    school = await repo.AllReadonly<Teacher>().Where(t => t.UserId == userId).Select(t => t.School.Id).FirstOrDefaultAsync();
            //}
            //else if (await roleService.UserIsInRoleAsync(userId.ToString(), "Principal")/*await repo.AllReadonly<Principal>().AnyAsync(p => p.UserId == userId)*/)//User is principal
            //{
            //    //school = await repo.AllReadonly<Principal>().Where(p => p.UserId == userId).Select(p => p.School.Id).FirstOrDefaultAsync();
            //    school = await repo.AllReadonly<School>().Where(s => s.PrincipalId == userId).Select(s => s.Id).FirstOrDefaultAsync();
            //}
            //else
            //{
            //    throw new ArgumentException("User is not a student, parent, teacher, or principal.");
            //}

        }

        public async Task UpdateSchoolAsync(SchoolViewModel school)
        {
            var schoolToUpdate = await repo.GetByIdAsync<School>(school.Id);

            if (schoolToUpdate is null)
            {
                throw new ArgumentException("School does not exist.");
            }

            schoolToUpdate.Name = school.Name;
            schoolToUpdate.Description = school.Description;
            schoolToUpdate.ImageUrl = school.ImageUrl!;
            schoolToUpdate.Location = school.Location;
            schoolToUpdate.PrincipalId = school.PrincipalId;

            await repo.SaveChangesAsync();
        }
    }
}
