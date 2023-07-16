using SchoolSocialMediaApp.ViewModels.Models.School;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface ISchoolService
    {

        /// <summary>
        /// Returns a list of all registered schools
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SchoolViewModel>> GetAllSchoolsAsync();

        /// <summary>
        /// Returns a school by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SchoolViewModel> GetSchoolByIdAsync(Guid id);

        /// <summary>
        /// Returns a school by user's id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<SchoolViewModel> GetSchoolByUserIdAsync(Guid userId);
        
        /// <summary>
        /// Returns school id by its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<SchoolViewModel> GetSchoolIdByNameAsync(string name);

        /// <summary>
        /// Returns school id by user's id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>SchoolViewModel</returns>
        /// <exception cref="InvalidOperationException">Thrown if the user is not a student, parent, teacher or principal</exception>
        Task<Guid> GetSchoolIdByUserIdAsync(Guid userId);

        /// <summary>
        /// Creates a new school and adds it to the database
        /// </summary>
        /// <param name="school"></param>
        /// <returns></returns>
        Task<SchoolViewModel> CreateSchoolAsync(SchoolViewModel school, Guid userId);

        /// <summary>
        /// Updates a school and saves the changes to the database
        /// </summary>
        /// <param name="school"></param>
        /// <returns></returns>
        Task UpdateSchoolAsync(SchoolViewModel school);

        /// <summary>
        /// Deletes a school from the database with all it's information (posts, comments, students, teachers, etc.)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteSchoolAsync(Guid id);
        Task<SchoolManageViewModel?> GetSchoolManageViewModelByUserIdAsync(Guid userId);
        Task AddUserToSchoolAsync(Guid userId, Guid schoolId);
        Task RemoveUserFromSchoolAsync(Guid userId, Guid schoolId);
    }
}
