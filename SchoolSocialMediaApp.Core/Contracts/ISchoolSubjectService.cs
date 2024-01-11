using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.SchoolClass;
using SchoolSocialMediaApp.ViewModels.Models.SchoolSubject;
using SchoolSocialMediaApp.ViewModels.Models.User;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface ISchoolSubjectService
    {
        Task AssignClassToSubject(Guid schoolId, Guid classId, Guid subjectId);
        Task AssignTeacherToSubject(Guid teacherId, Guid subjectId, Guid schoolId);
        Task<bool> CreateSchoolSubjectAsync(SchoolSubjectCreateModel model, Guid userId);
        Task DeleteSubject(Guid userId, Guid subjectId);
        Task<ICollection<SchoolClassViewModel>> GetAllAssignableToSubjectClassesAsync(Guid schoolId, Guid subjectId);
        Task<ICollection<SchoolSubjectViewModel>> GetAllSubjectsInSchoolAsync(Guid schoolId, Guid userId);
        Task<List<SchoolClassViewModel>> GetAssignedClasses(Guid subjectId);
        Task<ICollection<ApplicationUser>> GetCandidateTeachersInSchool(Guid schoolId, Guid userId, bool isAdmin = false);
        Task<List<TeacherViewModel>> GetCandidateTeachersViewModelInSchool(Guid schoolId, Guid userId, bool isAdmin);
        Task<SchoolSubjectViewModel> GetSubjectById(Guid subjectId);
        Task UnAssignClassFromSubject(Guid schoolId, Guid classId, Guid subjectId);
    }
}
