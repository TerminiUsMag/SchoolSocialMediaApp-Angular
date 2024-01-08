using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.SchoolClass;
using SchoolSocialMediaApp.ViewModels.Models.SchoolSubject;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface ISchoolClassService
    {
        Task<ICollection<SchoolClassViewModel>> GetAllClassesAsync(Guid schoolId, Guid userId);

        Task<SchoolClassViewModel> GetClassByIdAsync(Guid classId, Guid userId);

        Task<bool> CreateSchoolClassAsync(SchoolClassCreateModel schoolClassCreateModel, Guid userId, Guid schoolId);
        Task<List<ApplicationUser>> GetAllFreeStudentsAsync(Guid schoolId);
        Task AddStudentToClassAsync(Guid studentId, Guid classId);
        Task RemoveStudentFromClassAsync(Guid studentId, Guid classId);
        Task RemoveAllStudentsFromClassAsync(Guid classId);
        Task<Guid> RemoveAllSubjectsFromClassAndDeleteItAsync(Guid classId);
        Task<List<SchoolSubjectViewModel>> GetAllAssignableSubjectsToClassAsync(Guid classId, Guid schoolId);
        Task AddSubjectToClass(Guid schoolId, Guid classId, Guid subjectId);
        Task RemoveSubjectFromClassAsync(Guid subjectId, Guid classId, Guid schoolId);
    }
}
