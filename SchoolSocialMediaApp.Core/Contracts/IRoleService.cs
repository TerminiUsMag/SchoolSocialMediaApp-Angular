namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface IRoleService
    {
        Task<bool> CreateRoleAsync(string roleName);
        Task<bool> RoleExists(string roleName);
        Task<bool> AddUserToRole(string userId, string roleName);
        Task<bool> RemoveUserFromRole(string userId, string roleName);
    }
}
