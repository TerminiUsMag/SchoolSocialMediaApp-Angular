using SchoolSocialMediaApp.ViewModels.Models;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface ISchoolService
    {
        Task<IEnumerable<SchoolViewModel>> GetAllSchoolsAsync();
        Task<SchoolViewModel> GetSchoolByIdAsync(Guid id);
        Task<SchoolViewModel> GetSchoolByNameAsync(string name);
        Task<SchoolViewModel> GetSchoolByUserIdAsync(Guid userId);
        Task CreateSchoolAsync(SchoolViewModel school);
        Task UpdateSchoolAsync(SchoolViewModel school);
        Task DeleteSchoolAsync(Guid id);
    }
}
