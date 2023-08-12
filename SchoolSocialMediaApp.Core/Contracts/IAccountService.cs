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

        /// <summary>
        /// Returns information for the admin panel.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<AdminPanelViewModel> GetAdminPanelViewModel(Guid userId);

        /// <summary>
        /// Returns a delete user view model.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<AdminUserDeletionViewModel> GetAdminUserDeletionViewModelAsync(Guid userId);

        /// <summary>
        /// Checks if the phone is not used by anyone other than the userId.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> PhoneNumberIsFree(string phoneNumber, Guid userId);

        /// <summary>
        /// Checks if the email is not used by anyone other than the userId.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> EmailIsFree(string email, Guid userId);

        /// <summary>
        /// Checks if the username is not used by anyone other than the userId.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> UsernameIsFree(string username, Guid userId);

        /// <summary>
        /// Returns a view model which is used to make a user admin.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<MakeUserAdminViewModel> GetMakeUserAdminViewModelAsync(Guid userId);
        Task MakeAdmin(ApplicationUser userToMakeAdmin);
    }
}
