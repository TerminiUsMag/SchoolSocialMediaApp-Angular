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

        public async Task<bool> AddUserToRoleAsync(string userId, string roleName)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
            {
                throw new ArgumentException("User does not exist");
            }

            try
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);

                if (!roleExists)
                {
                    roleExists = await CreateRoleAsync(roleName);
                }

                if (!roleExists)
                {
                    throw new ArgumentException("Role could not be created.");
                }
                var result = await userManager.AddToRoleAsync(user, roleName);
                if (roleName.ToLower() == "principal")
                    user.IsPrincipal = true;
                else if (roleName.ToLower() == "teacher")
                    user.IsTeacher = true;
                else if (roleName.ToLower() == "student")
                    user.IsStudent = true;
                else if (roleName.ToLower() == "parent")
                    user.IsParent = true;
                else if (roleName.ToLower() == "admin")
                    user.IsAdmin = true;
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

        public async Task<List<string>> GetUserRolesAsync(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            var result = new List<string>();

            if (await userManager.IsInRoleAsync(user, "Principal"))
            {
                result.Add("Principal");
            }
            if (await userManager.IsInRoleAsync(user, "Teacher"))
            {
                result.Add("Teacher");
            }
            if (await userManager.IsInRoleAsync(user, "Student"))
            {
                result.Add("Student");
            }
            if (await userManager.IsInRoleAsync(user, "Parent"))
            {
                result.Add("Parent");
            }
            if (await userManager.IsInRoleAsync(user, "Admin"))
            {
                result.Add("Admin");
            }

            return result;
        }

        public bool IsUserPartOfSchool(ApplicationUser user)
        {
            var u = user;
            if (!u.IsParent && !u.IsPrincipal && !u.IsTeacher && !u.IsStudent && (u.SchoolId is null || u.SchoolId == Guid.Empty))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> RemoveUserFromRoleAsync(string userId, string roleName)
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

        public async Task<bool> RoleExistsAsync(string roleName)
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

        public async Task<bool> UserIsInRoleAsync(string userId, string roleName)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
            {
                throw new ArgumentException("User does not exist");
            }
            if (!await userManager.IsInRoleAsync(user, roleName))
            {
                return false;
            }
            return true;
        }
    }
}
