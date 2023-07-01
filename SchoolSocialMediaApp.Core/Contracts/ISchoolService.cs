using SchoolSocialMediaApp.ViewModels.Models;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface ISchoolService
    {
        Task<IEnumerable<SchoolViewModel>> GetAllSchoolsAsync();
        Task<SchoolViewModel> GetSchoolByIdAsync(Guid id);
        Task<SchoolViewModel> GetSchoolByNameAsync(string name);

        /// <summary>
        /// Returns the school of the user by their id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>SchoolViewModel</returns>
        /// <exception cref="InvalidOperationException">Thrown if the user is not a student, parent, teacher or principal</exception>
        Task<SchoolViewModel> GetSchoolByUserIdAsync(Guid userId);
        Task CreateSchoolAsync(SchoolViewModel school);
        Task UpdateSchoolAsync(SchoolViewModel school);
        Task DeleteSchoolAsync(Guid id);
    }
}
