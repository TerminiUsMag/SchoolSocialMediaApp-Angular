using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.School;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface ISchoolService
    {

        /// <summary>
        /// Gets all registered schools from the Database.
        /// </summary>
        /// <returns>List of SchoolViewModel</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<IEnumerable<SchoolViewModel>> GetAllSchoolsAsync();

        /// <summary>
        /// Get School by it's Id
        /// </summary>
        /// <param name="id">School ID</param>
        /// <returns>SchoolViewModel</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<SchoolViewModel> GetSchoolByIdAsync(Guid id);

        /// <summary>
        /// Get school by user ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>SchoolViewModel</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<SchoolViewModel> GetSchoolByUserIdAsync(Guid userId);

        /// <summary>
        /// Get school by Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>SchoolViewModel</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<SchoolViewModel> GetSchoolByNameAsync(string name);

        /// <summary>
        /// Get School's ID using User's ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>School's ID (Guid)</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<Guid> GetSchoolIdByUserIdAsync(Guid userId);

        /// <summary>
        /// Create a school from a SchoolViewModel with principalId -> userId and add it to the Database.
        /// </summary>
        /// <param name="model">A SchoolViewModel from which to create school</param>
        /// <param name="userId">Id of the principal of the school</param>
        /// <returns>SchoolViewModel</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<SchoolViewModel> CreateSchoolAsync(SchoolViewModel school, Guid userId);

        /// <summary>
        /// Update school in Database using a SchoolViewModel
        /// </summary>
        /// <param name="school"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task UpdateSchoolAsync(SchoolViewModel school);

        /// <summary>
        /// Delete school from Database using it's Id
        /// </summary>
        /// <param name="id">School Id</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task DeleteSchoolAsync(Guid id);

        /// <summary>
        /// Get SchoolViewModel for the Manage School Panel by User ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>ManageSchoolViewModel</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<SchoolManageViewModel?> GetSchoolManageViewModelByUserIdAsync(Guid userId);

        /// <summary>
        /// Add User to School using their IDs
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="schoolId">School ID</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task AddUserToSchoolAsync(Guid userId, Guid schoolId);

        /// <summary>
        /// Remove User from School using School's ID and user's ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="schoolId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task RemoveUserFromSchoolAsync(Guid userId, Guid schoolId);

        /// <summary>
        /// Rename school by ID
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="schoolName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task RenameSchoolAsync(Guid schoolId, string schoolName);

        /// <summary>
        /// Change school's profile picture using the principal's user ID and the path of the image.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task ChangeSchoolPicture(Guid userId, string imagePath);

        /// <summary>
        /// Update a School from the Manage Panel using SchoolManageViewModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task UpdateSchoolAsync(SchoolManageViewModel model, Guid userId);
        
        /// <summary>
        /// Returns a List of users which are registered in the school.
        /// </summary>
        /// <param name="schoolId"></param>
        /// <returns></returns>
        Task<List<ApplicationUser>> GetAllUsersInSchool(Guid schoolId);

        /// <summary>
        /// Get SchoolViewModel for the Manage School Admin Panel by school ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>ManageSchoolViewModel</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<SchoolManageViewModel?> GetSchoolManageViewModelBySchoolIdAsync(Guid schoolId);
        Task<AdminSchoolDeleteViewModel> GetAdminSchoolDeleteViewBySchoolIdAsync(Guid schoolId);
    }
}
