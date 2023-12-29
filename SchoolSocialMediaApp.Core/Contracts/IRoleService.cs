using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolSocialMediaApp.Infrastructure.Data.Models;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface IRoleService
    {
        /// <summary>
        /// Create a role.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>True or False</returns>
        Task<bool> CreateRoleAsync(string roleName);

        /// <summary>
        /// Checks if a role exists and if it doesn't exist it creates it.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>True or False</returns>
        Task<bool> RoleExistsAsync(string roleName);

        /// <summary>
        /// Adds user to role using User's ID and Role's Name
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns>True or False</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<bool> AddUserToRoleAsync(string userId, string roleName, string changingUserId = "");

        /// <summary>
        /// Remove User from Role using User's ID and Role's Name.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns>True or False</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<bool> RemoveUserFromRoleAsync(string userId, string roleName);

        /// <summary>
        /// Checks if User is in Role using User's ID and Role's Name
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns>True or False</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<bool> UserIsInRoleAsync(string userId, string roleName);

        /// <summary>
        /// Checks if the user is part of a school.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>True or False</returns>
        bool IsUserPartOfSchool(ApplicationUser user);

        /// <summary>
        /// Returns a List of all the User's Roles by his User ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A List of all roles of specific user.</returns>
        Task<List<string>> GetUserRolesAsync(Guid userId);
        Task<List<SelectListItem>> GetRolesAsync();
        Task AddUserToRoleIdAsync(Guid userId, Guid roleId, string changingUserId = "");
    }
}
