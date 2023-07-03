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

        public SchoolService(IRepository _repo)
        {
            this.repo = _repo;
        }

        public async Task<SchoolViewModel> CreateSchoolAsync(SchoolViewModel model, Guid userId)
        {
            var principal = await repo.AllReadonly<Principal>().FirstOrDefaultAsync(x => x.UserId == userId);

            if (principal is not null)
            {
                throw new ArgumentException("User is already a principal of another school.");
            }
            principal = new Principal
            {
                UserId = userId,
                Id = Guid.NewGuid()

            };

            var school = new School
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl!,
                Location = model.Location,
                Principal = principal,
                PrincipalId = principal.Id,
            };
            principal.School = school;
            principal.SchoolId = school.Id;

            await repo.AddAsync<Principal>(principal);
            await repo.AddAsync<School>(school);

            await repo.SaveChangesAsync();

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
                PrincipalId = s.PrincipalId
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
            Guid school = Guid.Empty;
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User id cannot be empty.");
            }

            if (await repo.AllReadonly<Student>().AnyAsync(s => s.UserId == userId))//User is student
            {
                school = await repo.AllReadonly<Student>().Where(s => s.UserId == userId).Select(s => s.School.Id).FirstOrDefaultAsync();
            }
            else if (await repo.AllReadonly<Parent>().AnyAsync(p => p.UserId == userId))//User is parent
            {
                school = await repo.AllReadonly<Parent>().Where(p => p.UserId == userId).Select(p => p.School.Id).FirstOrDefaultAsync();
            }
            else if (await repo.AllReadonly<Teacher>().AnyAsync(t => t.UserId == userId))//User is teacher
            {
                school = await repo.AllReadonly<Teacher>().Where(t => t.UserId == userId).Select(t => t.School.Id).FirstOrDefaultAsync();
            }
            else if (await repo.AllReadonly<Principal>().AnyAsync(p => p.UserId == userId))//User is principal
            {
                school = await repo.AllReadonly<Principal>().Where(p => p.UserId == userId).Select(p => p.School.Id).FirstOrDefaultAsync();
            }
            else
            {
                throw new ArgumentException("User is not a student, parent, teacher, or principal.");
            }

            if (school == Guid.Empty)
            {
                throw new ArgumentException("School id cannot be empty.");
            }
            return school;
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
