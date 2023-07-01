using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Common;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSocialMediaApp.Core.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly IRepository repo;

        public SchoolService(IRepository _repo)
        {
            this.repo = _repo;
        }

        public async Task CreateSchoolAsync(SchoolViewModel school)
        {
           await repo.AddAsync<School>(new School
            {
                Name = school.Name,
                Description = school.Description,
                ImageUrl = school.ImageUrl,
                Location = school.Location,
                PrincipalId = school.PrincipalId
            });

            await repo.SaveChangesAsync();
        }

        public async Task DeleteSchoolAsync(Guid id)
        {
            //await repo.GetByIdAsync<School>(id);
            await repo.DeleteAsync<School>(id);
            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<SchoolViewModel>> GetAllSchoolsAsync()
        {
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

        public async Task<SchoolViewModel> GetSchoolByNameAsync(string name)
        {
            var school = await repo.All<School>().FirstOrDefaultAsync(s => s.Name == name);
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
            School? school = null;

            if (await repo.AllReadonly<Student>().AnyAsync(s => s.UserId == userId))//User is student
            {
                school = await repo.AllReadonly<Student>().Where(s => s.UserId == userId).Select(s => s.School).FirstOrDefaultAsync();
            }
            else if (await repo.AllReadonly<Parent>().AnyAsync(p => p.UserId == userId))//User is parent
            {
                school = await repo.AllReadonly<Parent>().Where(p => p.UserId == userId).Select(p => p.School).FirstOrDefaultAsync();
            }
            else if (await repo.AllReadonly<Teacher>().AnyAsync(t => t.UserId == userId))//User is teacher
            {
                school = await repo.AllReadonly<Teacher>().Where(t => t.UserId == userId).Select(t => t.School).FirstOrDefaultAsync();
            }
            else if (await repo.AllReadonly<Principal>().AnyAsync(p => p.UserId == userId))//User is principal
            {
                school = await repo.AllReadonly<Principal>().Where(p => p.UserId == userId).Select(p => p.School).FirstOrDefaultAsync();
            }
            else
            {
                throw new InvalidOperationException("User is not a student, parent, teacher, or principal.");
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

        public Task UpdateSchoolAsync(SchoolViewModel school)
        {
            throw new NotImplementedException();
        }
    }
}
