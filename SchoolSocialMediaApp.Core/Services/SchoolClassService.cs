using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Common;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.School;
using SchoolSocialMediaApp.ViewModels.Models.SchoolClass;
using SchoolSocialMediaApp.ViewModels.Models.SchoolSubject;

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
                throw new ArgumentException("No school with this ID");

            var student = await repo.All<ApplicationUser>().FirstOrDefaultAsync(s => s.Id == studentId);
            if (student == null)
                throw new ArgumentException("No user with this ID");

            if (schoolClass.Students.Any(scs => scs.Id == studentId))
                throw new ArgumentException("The student is already assigned to this class");

            schoolClass.Students.Add(student);
            student.ClassId = classId;
            await repo.SaveChangesAsync();
        }

        public async Task<bool> CreateSchoolClassAsync(SchoolClassCreateModel schoolClassCreateModel, Guid userId, Guid schoolId)
        {
            SchoolViewModel school;
            try
            {
                var isAdmin = await roleService.UserIsInRoleAsync(userId.ToString(), "Admin");
                var isPrincipal = await roleService.UserIsInRoleAsync(userId.ToString(), "Principal");
                if (!isPrincipal && !isAdmin)
                    return false;

                if (schoolId == Guid.Empty)
                    school = await schoolService.GetSchoolByUserIdAsync(userId);
                else
                    school = await schoolService.GetSchoolByIdAsync(schoolId);

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
                    SchoolSubjects = new List<ClassesAndSubjects>(),
                };
                await repo.AddAsync(schoolClass);
                await repo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.Message == "The class you're trying to create already exists !" || ex.Message == "User is not a member of any school.")
                {
                    throw;
                }
                return false;
            }

            return true;
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
                     Subjects = sc.SchoolSubjects.Where(cas => cas.SchoolClassId == sc.Id).Select(cas => new SchoolSubjectViewModel
                     {
                         Id = cas.SchoolSubjectId
                     }).ToList(),
                     CreatedOn = sc.CreatedOn,
                 }).ToListAsync();

            return await GetSubjectInfoForClasses(classes);
        }
        private async Task<List<SchoolClassViewModel>> GetSubjectInfoForClasses(List<SchoolClassViewModel> classes)
        {
            foreach (var c in classes)
            {
                foreach (var subject in c.Subjects)
                {
                    var schoolSubject = await repo.All<SchoolSubject>().Include(ss => ss.School).ThenInclude(sss => sss.Principal).Include(ss => ss.Teacher).FirstOrDefaultAsync(ss => ss.Id == subject.Id);
                    if (schoolSubject is not null)
                    {
                        subject.SchoolId = schoolSubject.SchoolId;
                        subject.School = new SchoolViewModel
                        {
                            Id = schoolSubject.School.Id,
                            Description = schoolSubject.School.Description,
                            ImageUrl = schoolSubject.School.ImageUrl,
                            Location = schoolSubject.School.Location,
                            Name = schoolSubject.School.Name,
                            PrincipalId = schoolSubject.School.PrincipalId,
                            PrincipalName = $"{schoolSubject.School.Principal.FirstName} {schoolSubject.School.Principal.LastName}",
                        };
                        subject.TeacherId = schoolSubject.TeacherId;
                        subject.Teacher = schoolSubject.Teacher;
                        subject.CreatedOn = schoolSubject.CreatedOn;
                    }
                }
            }
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
                    Subjects = sc.SchoolSubjects.Select(cas => new SchoolSubjectViewModel
                    {
                        Id = cas.SchoolSubjectId,
                        Name = cas.SchoolSubject.Name
                    }).ToList(),
                })
                .FirstOrDefaultAsync();

            if (schoolClass is null)
            {
                throw new ArgumentException("School class with this Id does not exist ( " + classId + " )");
            }

            foreach (var subject in schoolClass.Subjects)
            {

                var schoolSubject = await repo.All<SchoolSubject>().Include(ss => ss.School).ThenInclude(sss => sss.Principal).Include(ss => ss.Teacher).FirstOrDefaultAsync(ss => ss.Id == subject.Id);
                if (schoolSubject is not null)
                {
                    subject.SchoolId = schoolSubject.SchoolId;
                    subject.School = new SchoolViewModel
                    {
                        Id = schoolSubject.School.Id,
                        Description = schoolSubject.School.Description,
                        ImageUrl = schoolSubject.School.ImageUrl,
                        Location = schoolSubject.School.Location,
                        Name = schoolSubject.School.Name,
                        PrincipalId = schoolSubject.School.PrincipalId,
                        PrincipalName = $"{schoolSubject.School.Principal.FirstName} {schoolSubject.School.Principal.LastName}"
                    };
                    subject.TeacherId = schoolSubject.TeacherId;
                    subject.Teacher = schoolSubject.Teacher;
                    subject.CreatedOn = schoolSubject.CreatedOn;
                }
            }

            return schoolClass;
        }

        public async Task RemoveAllStudentsFromClassAsync(Guid classId)
        {
            var schoolClass = await repo.All<SchoolClass>().Include(sc => sc.Students).FirstOrDefaultAsync(sc => sc.Id == classId);
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

        public async Task<Guid> RemoveAllSubjectsFromClassAndDeleteItAsync(Guid classId)
        {
            var schoolClass = await repo.All<SchoolClass>().Include(sc => sc.Students).Include(sc => sc.SchoolSubjects).FirstOrDefaultAsync(sc => sc.Id == classId);

            if (schoolClass is null)
            {
                throw new ArgumentException("No School Class found!");
            }

            while (schoolClass.SchoolSubjects.Count > 0)
            {
                var classAndSubject = schoolClass.SchoolSubjects.First();

                repo.Delete(classAndSubject);
                schoolClass.SchoolSubjects.Remove(classAndSubject);
            }
            try
            {
                if (schoolClass is not null && !schoolClass.Students.Any() && !schoolClass.SchoolSubjects.Any())
                {
                    repo.Delete(schoolClass);
                    await repo.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return schoolClass.SchoolId;
        }

        public async Task<List<SchoolSubjectViewModel>> GetAllAssignableSubjectsToClassAsync(Guid classId, Guid schoolId)
        {
            if (classId == Guid.Empty)
                throw new ArgumentException("The class id cannot be empty");

            var schoolClass = await repo.All<SchoolClass>().Include(sc => sc.SchoolSubjects).FirstOrDefaultAsync(sc => sc.Id == classId && sc.SchoolId == schoolId);

            if (schoolClass is null)
                throw new ArgumentException("No class with this Id in the school");

            var subjectsAlreadyAssignedToClass = schoolClass.SchoolSubjects;

            var subjects = await repo.All<SchoolSubject>().Where(ss => ss.SchoolId == schoolId).Include(ss => ss.Teacher).ToListAsync();
            List<SchoolSubject> freeSubjects = new List<SchoolSubject>();
            foreach (var subject in subjects)
            {
                if (!subjectsAlreadyAssignedToClass.Any(ss => ss.SchoolSubjectId == subject.Id))
                {
                    freeSubjects.Add(subject);
                }
            }

            return freeSubjects.Select(fs => new SchoolSubjectViewModel
            {
                TeacherName = $"{fs.Teacher!.FirstName} {fs.Teacher.LastName}",
                CreatedOn = fs.CreatedOn,
                Id = fs.Id,
                Name = fs.Name,
                SchoolId = fs.SchoolId,
                TeacherId = fs.TeacherId,
            }).ToList();
        }

        public async Task AddSubjectToClass(Guid schoolId, Guid classId, Guid subjectId)
        {
            if (schoolId == Guid.Empty || classId == Guid.Empty || subjectId == Guid.Empty)
                throw new ArgumentException("At least one argument is wrong");

            var schoolClass = await repo.All<SchoolClass>().Include(sc => sc.SchoolSubjects).FirstOrDefaultAsync(sc => sc.Id == classId && sc.SchoolId == schoolId);

            if (schoolClass is null)
                throw new ArgumentException("No such class found in this school");

            var schoolSubject = await repo.All<SchoolSubject>().FirstOrDefaultAsync(ss => ss.Id == subjectId && ss.SchoolId == schoolId);

            if (schoolSubject is null)
                throw new ArgumentException("No such subject found in this school");

            if (schoolClass.SchoolSubjects.Any(ss => ss.SchoolSubjectId == schoolSubject.Id))
                throw new ArgumentException("That subject is already added to this class");

            var newClassSubject = new ClassesAndSubjects
            {
                SchoolClassId = schoolClass.Id,
                SchoolSubjectId = schoolSubject.Id,
            };

            schoolSubject.SchoolClasses.Add(newClassSubject);
            schoolClass.SchoolSubjects.Add(newClassSubject);

            await repo.SaveChangesAsync();
        }

        public async Task RemoveSubjectFromClassAsync(Guid subjectId, Guid classId, Guid schoolId)
        {
            if (subjectId == Guid.Empty || classId == Guid.Empty)
                throw new ArgumentException("At least one argument is empty");

            var subject = await repo.All<SchoolSubject>().Include(s => s.SchoolClasses).FirstOrDefaultAsync(s => s.Id == subjectId && s.SchoolId == schoolId);

            if (subject is null)
                throw new ArgumentException("No such subject found in this school");

            var schoolClass = await repo.All<SchoolClass>().Include(c => c.SchoolSubjects).FirstOrDefaultAsync(c => c.Id == classId && c.SchoolId == schoolId);

            if (schoolClass is null)
                throw new ArgumentException("No such class found in this school");

            var subjectClass = subject.SchoolClasses.FirstOrDefault(sc => sc.SchoolClassId == schoolClass.Id);
            var classSubject = schoolClass.SchoolSubjects.FirstOrDefault(cs => cs.SchoolSubjectId == subject.Id);

            if (classSubject is null || subjectClass is null)
                throw new ArgumentException("Subject and Class are not linked in any way");

            subject.SchoolClasses.Remove(subjectClass);
            schoolClass.SchoolSubjects.Remove(classSubject);

            await repo.SaveChangesAsync();
        }
    }
}
