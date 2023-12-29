using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.SchoolClass;
using SchoolSocialMediaApp.ViewModels.Models.SchoolSubject;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface ISchoolSubjectService
    {
        Task AssignClassToSubject(Guid schoolId, Guid classId, Guid subjectId);
        Task<bool> CreateSchoolSubjectAsync(SchoolSubjectCreateModel model, Guid userId);
        Task DeleteSubject(Guid userId, Guid subjectId);
        Task<ICollection<SchoolClassViewModel>> GetAllAssignableToSubjectClassesAsync(Guid schoolId, Guid subjectId);
        Task<ICollection<SchoolSubjectViewModel>> GetAllSubjectsInSchoolAsync(Guid schoolId, Guid userId);
        Task<ICollection<ApplicationUser>> GetCandidateTeachersInSchool(Guid schoolId, Guid userId, bool isAdmin = false);
        Task<SchoolSubjectViewModel> GetSubjectById(Guid subjectId);
        Task UnAssignClassFromSubject(Guid schoolId, Guid classId, Guid subjectId);
    }
}
