using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.User;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface IAccountService
    {

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> RegisterAsync(ApplicationUser user, string password);

        /// <summary>
        /// LogIn asynchronously.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="rememberMe"></param>
        /// <returns></returns>
        Task<bool> LoginAsync(string email, string password, bool rememberMe);

        /// <summary>
        /// LogOut asynchronously.
        /// </summary>
        /// <returns></returns>
        Task LogoutAsync();

        /// <summary>
        /// Checks if the email address is valid
        /// </summary>
        /// <param name="email"></param>
        /// <returns>True or False</returns>
        bool EmailIsValid(string email);

        /// <summary>
        /// Checks if the phoneNumber is valid.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        bool PhoneNumberIsValid(string phoneNumber);

        /// <summary>
        /// Checks if the email address is free(not used by another user).
        /// </summary>
        /// <param name="email"></param>
        /// <returns>True or False</returns>
        Task<bool> EmailIsFree(string email);

        /// <summary>
        /// Checks if the phoneNumber is free (not used by another user).
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task<bool> PhoneNumberIsFree(string phoneNumber);

        /// <summary>
        /// Checks if the username is free(Not used by another user).
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<bool> UsernameIsFree(string username);

        /// <summary>
        /// Checks if user with this Id exists.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> UserExists(Guid userId);

        /// <summary>
        /// Gets the User by id and maps it to a UserManageViewModel which is used for the account management functionality.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task<UserManageViewModel> GetUserManageViewModelAsync(string userId);

        /// <summary>
        /// Updates User's information.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task UpdateAsync(Guid userId, UserManageViewModel model);
        Task<AdminPanelViewModel> GetAdminPanelViewModel(Guid userId);
        Task<AdminUserDeletionViewModel> GetAdminUserDeletionViewModelAsync(Guid userId);
        Task<bool> PhoneNumberIsFree(string phoneNumber, Guid userId);
        Task<bool> EmailIsFree(string email, Guid userId);
        Task<bool> UsernameIsFree(string username, Guid userId);
    }
}
