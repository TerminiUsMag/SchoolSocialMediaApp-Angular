using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.SchoolClass;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface ISchoolClassService
    {
        Task<ICollection<SchoolClassViewModel>> GetAllClassesAsync(Guid schoolId, Guid userId);

        Task<SchoolClassViewModel> GetClassByIdAsync(Guid classId, Guid userId);

        Task<bool> CreateSchoolClassAsync(SchoolClassCreateModel schoolClassCreateModel, Guid userId);
        Task<bool> DeleteClassAsync(Guid classId, Guid userId);
        Task<List<ApplicationUser>> GetAllFreeStudentsAsync(Guid schoolId);
        Task AddStudentToClassAsync(Guid studentId, Guid classId);
        Task RemoveStudentFromClassAsync(Guid studentId, Guid classId);
        Task RemoveAllStudentsFromClassAsync(Guid classId);
    }
}
