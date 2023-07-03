using Microsoft.AspNetCore.Identity;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Data.Models;

namespace SchoolSocialMediaApp.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RoleService(
            RoleManager<ApplicationRole> _roleManager,
            UserManager<ApplicationUser> _userManager)
        {
            this.roleManager = _roleManager;
            userManager = _userManager;
        }

        public async Task<bool> AddUserToRole(string userId, string roleName)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
            {
                throw new ArgumentException("User does not exist");
            }

            try
            {
                var result = await userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            var role = new ApplicationRole { Name = roleName };
            var result = await roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> RemoveUserFromRole(string userId, string roleName)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
            {
                throw new ArgumentException("User does not exist");
            }

            try
            {
                var result = await userManager.RemoveFromRoleAsync(user, roleName);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RoleExists(string roleName)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                if (!await CreateRoleAsync(roleName))
                {
                    return false;
                }
            }
            return true;
        }


    }
}
