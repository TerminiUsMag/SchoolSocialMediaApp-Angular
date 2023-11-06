using SchoolSocialMediaApp.ViewModels.Models.SchoolClass;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface ISchoolClassService
    {
        Task<ICollection<SchoolClassViewModel>> GetAllClassesAsync(Guid schoolId, Guid userId);

        Task<SchoolClassViewModel> GetClassByIdAsync(Guid classId);

        Task<bool> CreateSchoolClassAsync(SchoolClassCreateModel schoolClassCreateModel, Guid userId);
        Task<bool> DeleteClass(Guid classId, Guid userId);
    }
}
