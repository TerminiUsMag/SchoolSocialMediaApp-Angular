using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Common;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.User;
using System.Net.Mail;
using System.Text.RegularExpressions;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;

namespace SchoolSocialMediaApp.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IWebHostEnvironment env;
        private readonly IRoleService roleService;
        private readonly IRepository repo;
        public AccountService(
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager,
            IRepository _repo,
            IRoleService _roleService,
            IWebHostEnvironment _env)
        {
            this.userManager = _userManager;
            this.signInManager = _signInManager;
            this.repo = _repo;
            this.roleService = _roleService;
            this.env = _env;
        }

        public Task<bool> DeleteAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EmailIsFree(string email)
        {
            var result = await userManager.FindByEmailAsync(email.ToUpper());

            if (result is null)
            {
                return true;
            }
            return false;
        }

        public bool EmailIsValid(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public async Task<UserManageViewModel> GetUserManageViewModelAsync(string userId)
        {
            if (userId is null || userId == Guid.Empty.ToString())
            {
                throw new ArgumentException("User does not exist");
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new ArgumentException("User does not exist");
            }

            var model = new UserManageViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ImageUrl = user.ImageUrl,
            };

            return model;
        }

        public async Task<bool> IsPrincipalAsync(Guid userId)
        {
            var isPrincipal = await roleService.UserIsInRoleAsync(userId.ToString(), "Principal");

            if (isPrincipal)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> LoginAsync(string email, string password, bool rememberMe)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return false;
            }
            var result = await signInManager.PasswordSignInAsync(user, password, rememberMe, true);
            if (result.Succeeded)
            {
                signInManager.Logger.LogInformation("User logged in.");
                return true;
            }
            return false;
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<bool> PhoneNumberIsFree(string phoneNumber)
        {
            var result = await repo.AllReadonly<ApplicationUser>().FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);

            if (result is null)
            {
                return true;
            }
            return false;
        }

        public bool PhoneNumberIsValid(string phoneNumber)
        {
            var phoneRegex = new Regex(validation.PhoneNumberRegEx);
            if (!phoneRegex.IsMatch(phoneNumber))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> RegisterAsync(ApplicationUser user, string password)
        {
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                if (await roleService.RoleExistsAsync("User"))
                    await userManager.AddToRoleAsync(user, "User");

                await signInManager.SignInAsync(user, isPersistent: false);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(Guid userId, UserManageViewModel model)
        {
            var result = false;
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                return result;
            }


            // Check if a profile picture was uploaded
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                // Generate a unique file name for the uploaded picture
                string uniqueFileName = model.Id.ToString() + "_" + Guid.NewGuid().ToString() + ".jpg";

                //// Set the path to save the file (wwwroot/user-images)
                //string imagePath = $"/images/user-images/{uniqueFileName}";

                // Set the path to save the file (wwwroot/images/user-images)
                string imagePath = Path.Combine(env.WebRootPath, "images", "user-images", uniqueFileName);

                // Save the uploaded file to the server
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }

                // Update the user's profile with the file path or uniqueFileName as needed
                // For example, you might store the file path in a database or link it to the user's profile.
                // Your implementation will depend on your data storage strategy.
                model.ImageUrl = $"images/user-images/{uniqueFileName}";
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = $"{model.FirstName.ToLower()}.{model.LastName.ToLower()}";
            user.NormalizedUserName = user.UserName.ToUpper();
            user.Email = model.Email;
            user.NormalizedEmail = model.Email.ToUpper();
            user.PhoneNumber = model.PhoneNumber;
            user.ImageUrl = model.ImageUrl;
            user.NormalizedEmail = model.Email.ToUpper();

            await repo.SaveChangesAsync();
            result = true;

            return result;
        }

        public async Task<bool> UserExists(Guid userId)
        {
            if (await repo.AllReadonly<ApplicationUser>().FirstOrDefaultAsync(x => x.Id == userId) is null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UsernameIsFree(string username)
        {
            var result = await repo.AllReadonly<ApplicationUser>().FirstOrDefaultAsync(x => x.NormalizedUserName == username.ToUpper());

            if (result is null)
            {
                return true;
            }
            return false;
        }
    }
}
