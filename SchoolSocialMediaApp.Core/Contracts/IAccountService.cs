using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.User;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface IAccountService
    {
        Task<bool> RegisterAsync(ApplicationUser user, string password);
        Task<bool> LoginAsync(string email, string password, bool rememberMe);
        Task LogoutAsync();
        bool EmailIsValid(string email);
        bool PhoneNumberIsValid(string phoneNumber);
        Task<bool> EmailIsFree(string email);
        Task<bool> PhoneNumberIsFree(string phoneNumber);
        Task<bool> UsernameIsFree(string username);
        Task<bool> DeleteAsync(Guid userId);
        Task<bool> UserExists(Guid userId);
        Task<UserManageViewModel> GetUserManageViewModelAsync(string userId);
        Task<bool> UpdateAsync(Guid userId, UserManageViewModel model);
    }
}
