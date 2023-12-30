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
        private readonly IRoleService roleService;
        private readonly ISchoolService schoolService;
        public SchoolSubjectService(IRepository _repo, IRoleService _roleService, ISchoolService _schoolService)
        {

            this.repo = _repo;
            this.roleService = _roleService;
            this.schoolService = _schoolService;

        }

        public async Task AssignClassToSubject(Guid schoolId, Guid classId, Guid subjectId)
        {
            var school = await repo
                .All<School>()
                .Where(s => s.Id == schoolId)
                .FirstOrDefaultAsync();

            if (school is null)
            {
                throw new ArgumentException("No school with this Id is registered");
            }

            var schoolClass = await repo
                .All<SchoolClass>()
                .Where(sc => sc.Id == classId)
                .Include(sc => sc.SchoolSubjects)
                .FirstOrDefaultAsync();

            if (schoolClass is null)
            {
                throw new ArgumentException("No school class with this Id");
            }

            var subject = await repo
                .All<SchoolSubject>()
                .Where(ss => ss.Id == subjectId)
                .Include(ss => ss.SchoolClasses)
                .FirstOrDefaultAsync();

            if (subject is null)
            {
                throw new ArgumentException("No school subject with this Id");
            }

            if (schoolClass.SchoolId != school.Id)
            {
                throw new ArgumentException("The class is not part of this school");
            }

            if (subject.SchoolId != school.Id)
            {
                throw new ArgumentException("The subject is not part of this school");
            }

            var newSchoolClassSubject = new ClassesAndSubjects { SchoolClassId = schoolClass.Id, SchoolSubjectId = subject.Id };

            schoolClass.SchoolSubjects.Add(newSchoolClassSubject);
            subject.SchoolClasses.Add(newSchoolClassSubject);
            await repo.SaveChangesAsync();
        }

        public async Task<bool> CreateSchoolSubjectAsync(SchoolSubjectCreateModel model, Guid userId)
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
                    .AllReadonly<SchoolSubject>()
                    .AnyAsync(ss => ss.Name == model.Name && ss.SchoolId == schoolId && ss.TeacherId == model.TeacherId);
                if (duplicate)
                {
                    throw new ArgumentException("The subject you're trying to create already exists !");
                }

                var schoolSubject = new SchoolSubject
                {
                    Id = new Guid(),
                    CreatedOn = DateTime.Now,
                    Name = model.Name,
                    SchoolId = schoolId,
                    SchoolClasses = new List<ClassesAndSubjects>(),
                    TeacherId = model.TeacherId,
                };
                await repo.AddAsync(schoolSubject);
                await repo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.Message == "The subject you're trying to create already exists !")
                {
                    throw;
                }
                return false;
            }

            return true;
        }

        public async Task DeleteSubject(Guid userId, Guid subjectId)
        {

            var subject = await repo
                .All<SchoolSubject>()
                .Where(ss => ss.Id == subjectId)
                .FirstOrDefaultAsync();

            if (subject is null)
            {
                throw new ArgumentException("No school subject with this Id");
            }
            var schoolPrincipalId = subject.School.PrincipalId;
            var schoolPrincipal = subject.School.Principal;

            //.Include(ss=>ss.School)
            //.ThenInclude(s=>s.Principal)
            //.Include(ss => ss.SchoolClasses)
            //.ThenInclude(sc => sc.SchoolClass)
            //.Include(ss => ss.Teacher)


            var schoolSubjectClasses = await repo.All<ClassesAndSubjects>().Where(cas => cas.SchoolSubjectId == subjectId).ToListAsync();




            var user = await repo
                .All<ApplicationUser>()
                .Where(u => u.Id == userId)
                .Include(u => u.School)
                .FirstOrDefaultAsync();

            if (user is null)
            {
                throw new ArgumentException("No such user");
            }

            var schoolId = subject.SchoolId;
            var school = await repo.All<School>().Where(s => s.Id == schoolId).FirstOrDefaultAsync();

            if (school is null)
            {
                throw new ArgumentException("No school with this Id is registered");
            }
            if (subject.SchoolId != school.Id)
            {
                throw new ArgumentException("The subject is not part of this school");
            }

            while (subject.SchoolClasses.Count > 0)
            {
                var schoolClassAndSubject = subject.SchoolClasses.First();
                //var schoolClass = schoolClassAndSubject!.SchoolClass;
                //schoolClass!.SchoolSubjects.Remove(schoolClassAndSubject);

                repo.Delete(schoolClassAndSubject);
                subject.SchoolClasses.Remove(schoolClassAndSubject);
            }

            if (subject.TeacherId is not null || subject.TeacherId != Guid.Empty)
            {
                var teacher = await repo.All<ApplicationUser>().Where(t => t.IsTeacher && t.Id == subject.TeacherId).Include(t => t.Subjects).FirstOrDefaultAsync();
                if (teacher is null)
                {
                    throw new ArgumentException("The teacher could not be found.");
                }

                teacher.Subjects.Remove(subject);
                subject.Teacher = null;
                subject.TeacherId = null;

                teacher.SchoolId = school.Id;
                user.SchoolId = school.Id;
                school.PrincipalId = schoolPrincipalId;
                school.Principal = schoolPrincipal;
                //subject.School.PrincipalId = schoolPrincipalId;
                //subject.School.Principal = schoolPrincipal;
                await repo.SaveChangesAsync();

                teacher.SchoolId = school.Id;
            }

            repo.Delete(subject);
            await repo.SaveChangesAsync();
        }

        public async Task<ICollection<SchoolClassViewModel>> GetAllAssignableToSubjectClassesAsync(Guid schoolId, Guid subjectId)
        {
            if (schoolId == Guid.Empty || subjectId == Guid.Empty)
            {
                throw new ArgumentException("At least one of the arguments is empty");
            }

            var classes = await repo
                .All<SchoolClass>()
                .Where(sc => sc.SchoolId == schoolId && !sc.SchoolSubjects.Any(scss => scss.SchoolClassId == sc.Id && scss.SchoolSubjectId == subjectId))
                .Select(sc => new SchoolClassViewModel
                {
                    CreatedOn = sc.CreatedOn,
                    Grade = sc.Grade,
                    Id = sc.Id,
                    Name = sc.Name,
                    SchoolId = sc.SchoolId,
                    Students = sc.Students
                })
                .ToListAsync();

            if (classes is null)
            {
                throw new ArgumentException("No Assignable classes for this subject");
            }

            return classes;
        }

        public async Task<ICollection<SchoolSubjectViewModel>> GetAllSubjectsInSchoolAsync(Guid schoolId, Guid userId)
        {
            if (schoolId == Guid.Empty)
            {
                schoolId = await schoolService.GetSchoolIdByUserIdAsync(userId);
            }

            var subjectsInSchoolRaw = await repo
                .All<SchoolSubject>()
                .Where(ss => ss.SchoolId == schoolId)
                .Include(ss => ss.School)
                //.ThenInclude(sss => sss.Principal)
                .Include(ss => ss.Teacher)
                .Include(ss => ss.SchoolClasses)
                .ToListAsync();

            // Check the raw data in subjectsInSchoolRaw and inspect it for duplicates

            var subjectsInSchool = subjectsInSchoolRaw
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
                        //PrincipalName = $"{ss.School.Principal.FirstName} {ss.School.Principal.LastName}",
                    },
                    CreatedOn = ss.CreatedOn,
                    Id = ss.Id,
                    TeacherId = ss.TeacherId,
                    Teacher = ss.Teacher,
                    Name = ss.Name,
                    Classes = ss.SchoolClasses.Select(cas => new SchoolClassViewModel
                    {
                        Id = cas.SchoolClassId,
                    })
                .ToList(),
                }).ToList();


            //var subjectsInSchool = await repo
            //    .All<SchoolSubject>()
            //    .Where(ss => ss.SchoolId == schoolId)
            //    .Include(ss => ss.School)
            //    .ThenInclude(sss => sss.Principal)
            //    .Select(ss => new SchoolSubjectViewModel
            //    {
            //        SchoolId = ss.SchoolId,
            //        School = new SchoolViewModel
            //        {
            //            Description = ss.School.Description,
            //            Id = ss.School.Id,
            //            ImageUrl = ss.School.ImageUrl,
            //            Location = ss.School.Location,
            //            Name = ss.School.Name,
            //            PrincipalId = ss.School.PrincipalId,
            //            PrincipalName = $"{ss.School.Principal.FirstName} {ss.School.Principal.LastName}",
            //        },
            //        CreatedOn = ss.CreatedOn,
            //        Id = ss.Id,
            //        TeacherId = ss.TeacherId,
            //        Name = ss.Name,
            //        Classes = ss.SchoolClasses.Select(cas => new SchoolClassViewModel
            //        {
            //            Id = cas.SchoolClassId,
            //        }).ToList(),
            //    })
            //    .ToListAsync();

            foreach (var subject in subjectsInSchool)
            {
                //subject.Teacher = await repo.All<ApplicationUser>().FirstOrDefaultAsync(t => t.Id == subject.TeacherId);
                var principal = await repo.All<ApplicationUser>().FirstOrDefaultAsync(p => p.Id == subject.School.PrincipalId);
                subject.School.PrincipalName = $"{principal!.FirstName} {principal.LastName}";

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

        public async Task<ICollection<ApplicationUser>> GetCandidateTeachersInSchool(Guid schoolId, Guid userId, bool isAdmin = false)
        {
            var school = await repo.All<School>().Where(sc => sc.Id == schoolId).FirstOrDefaultAsync();

            if (school is null)
            {
                throw new ArgumentException("A school with this Id doesn't exist");
            }
            if (school.PrincipalId != userId && !isAdmin)
                throw new ArgumentException("You aren't the school's principal");

            var teachers = await repo.All<ApplicationUser>().Where(au => au.IsTeacher == true && au.SchoolId == school.Id).ToListAsync();


            return teachers;
        }

        public async Task<SchoolSubjectViewModel> GetSubjectById(Guid subjectId)
        {
            if (subjectId == Guid.Empty)
            {
                throw new ArgumentException("subjectId is empty");
            }

            var result = await repo
             .All<SchoolSubject>()
             .Where(ss => ss.Id == subjectId)
             .Select(ss => new SchoolSubjectViewModel
             {
                 Id = ss.Id,
                 CreatedOn = ss.CreatedOn,
                 Name = ss.Name,
                 SchoolId = ss.SchoolId,
                 TeacherId = ss.TeacherId,
                 Teacher = ss.Teacher,
                 Classes = ss.SchoolClasses.Select(sc => new SchoolClassViewModel
                 {
                     CreatedOn = sc.SchoolClass.CreatedOn,
                     Grade = sc.SchoolClass.Grade,
                     Id = sc.SchoolClass.Id,
                     Name = sc.SchoolClass.Name,
                     Students = sc.SchoolClass.Students,
                     SchoolId = sc.SchoolClassId,
                 }).ToList(),
             }).FirstOrDefaultAsync();

            if (result is null)
            {
                throw new ArgumentException("No subject with this subjectId");
            }

            return result;
        }

        public async Task UnAssignClassFromSubject(Guid schoolId, Guid classId, Guid subjectId)
        {
            var school = await repo
                .All<School>()
                .Where(s => s.Id == schoolId)
                .FirstOrDefaultAsync();

            if (school is null)
            {
                throw new ArgumentException("No school with this Id is registered");
            }

            var schoolClass = await repo.All<SchoolClass>()
                .Where(sc => sc.Id == classId)
                .Include(sc => sc.SchoolSubjects)
                .FirstOrDefaultAsync();

            if (schoolClass is null)
            {
                throw new ArgumentException("No school class with this Id");
            }

            var subject = await repo.All<SchoolSubject>()
                .Where(ss => ss.Id == subjectId)
                .Include(ss => ss.SchoolClasses)
                .FirstOrDefaultAsync();

            if (subject is null)
            {
                throw new ArgumentException("No school subject with this Id");
            }

            if (schoolClass.SchoolId != school.Id)
            {
                throw new ArgumentException("The class is not part of this school");
            }

            if (subject.SchoolId != school.Id)
            {
                throw new ArgumentException("The subject is not part of this school");
            }

            var schoolClassAndSubject = await repo
                .All<ClassesAndSubjects>()
                .Where(cas => cas.SchoolClassId == schoolClass.Id && cas.SchoolSubjectId == subject.Id)
                .FirstOrDefaultAsync();

            if (schoolClassAndSubject is null)
            {
                throw new ArgumentException("The class is not assigned to this subject");
            }

            schoolClass.SchoolSubjects.Remove(schoolClassAndSubject);
            subject.SchoolClasses.Remove(schoolClassAndSubject);
            repo.Delete(schoolClassAndSubject);
            await repo.SaveChangesAsync();
        }
    }
}
