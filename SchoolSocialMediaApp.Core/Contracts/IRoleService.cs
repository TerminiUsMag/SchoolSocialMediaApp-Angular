using SchoolSocialMediaApp.Infrastructure.Data.Models;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface IRoleService
    {
        Task<bool> CreateRoleAsync(string roleName);
        Task<bool> RoleExistsAsync(string roleName);
        Task<bool> AddUserToRoleAsync(string userId, string roleName);
        Task<bool> RemoveUserFromRoleAsync(string userId, string roleName);
        Task<bool> UserIsInRoleAsync(string userId, string roleName);
        bool IsUserPartOfSchool(ApplicationUser user);
        Task<List<string>> GetUserRolesAsync(Guid userId);
    }
}
