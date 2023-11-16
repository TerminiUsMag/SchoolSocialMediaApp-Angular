using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Common;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.School;
using SchoolSocialMediaApp.ViewModels.Models.SchoolClass;
using SchoolSocialMediaApp.ViewModels.Models.SchoolSubject;

namespace SchoolSocialMediaApp.Core.Services
{
    public class SchoolSubjectService : ISchoolSubjectService
    {
        private readonly IRepository repo;
        public SchoolSubjectService(IRepository _repo)
        {

            this.repo = _repo;

        }

        public async Task<ICollection<SchoolSubjectViewModel>> GetAllSubjectsInSchoolAsync(Guid schoolId)
        {
            var subjectsInSchool = await repo
                .All<SchoolSubject>()
                .Where(ss => ss.SchoolId == schoolId)
                .Include(ss => ss.School)
                .ThenInclude(sss => sss.Principal)
                .Include(ss => ss.Teacher)
                .Select(ss => new SchoolSubjectViewModel
                {
                    SchoolId = ss.SchoolId,
                    School = new SchoolViewModel
                    {
                        Description = ss.School.Description,
                        Id = ss.School.Id,
                        ImageUrl = ss.School.ImageUrl,
                        Location = ss.School.Location,
                        Name = ss.School.Name,
                        PrincipalId = ss.School.PrincipalId,
                        PrincipalName = $"{ss.School.Principal.FirstName} {ss.School.Principal.LastName}",
                    },
                    CreatedOn = ss.CreatedOn,
                    Id = ss.Id,
                    TeacherId = ss.TeacherId,
                    Teacher = ss.Teacher,
                    Name = ss.Name,
                    Classes = ss.SchoolClasses.Select(cas => new SchoolClassViewModel
                    {
                        Id = cas.SchoolClassId,
                    }).ToList(),
                }).ToListAsync();

            foreach (var subject in subjectsInSchool)
            {

                foreach (var schoolClass in subject.Classes)
                {
                    var referenceClass = await repo
                        .All<SchoolClass>()
                        .Include(sc => sc.School)
                        .ThenInclude(scs => scs.Principal)
                        .FirstOrDefaultAsync(sc => sc.Id == schoolClass.Id);

                    if (referenceClass is not null)
                    {
                        schoolClass.Name = referenceClass.Name;
                        schoolClass.Students = referenceClass.Students;
                        schoolClass.School = new SchoolViewModel
                        {
                            Description = referenceClass.School.Description,
                            Id = referenceClass.School.Id,
                            ImageUrl = referenceClass.School.ImageUrl,
                            Location = referenceClass.School.Location,
                            Name = referenceClass.School.Name,
                            PrincipalId = referenceClass.School.PrincipalId,
                            PrincipalName = $"{referenceClass.School.Principal.FirstName} {referenceClass.School.Principal.LastName}",
                        };
                        schoolClass.SchoolId = referenceClass.SchoolId;
                        schoolClass.Grade = referenceClass.Grade;
                        schoolClass.Id = referenceClass.Id;
                        schoolClass.CreatedOn = referenceClass.CreatedOn;
                    }
                }
            }

            return subjectsInSchool;
        }
    }
}
