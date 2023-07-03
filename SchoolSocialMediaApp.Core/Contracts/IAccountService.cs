using SchoolSocialMediaApp.Infrastructure.Data.Models;

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
        Task<bool> CreateTeacherAsync(Guid userId);
        Task<bool> CreateStudentAsync(Guid userId);
        Task<bool> CreateParentAsync(Guid userId);
        Task<bool> IsPrincipalAsync(Guid userId);
        Task<bool> IsTeacherAsync(Guid userId);
        Task<bool> IsParentAsync(Guid userId);
        Task<bool> IsStudentAsync(Guid userId);
        Task<bool> UserExists(Guid userId);

    }
}
