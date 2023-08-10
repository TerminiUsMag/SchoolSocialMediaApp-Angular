using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Common;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.Post;
using SchoolSocialMediaApp.ViewModels.Models.School;
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
            this.roleService = _roleService;
            this.repo = _repo;
            this.env = _env;
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

        public async Task<AdminPanelViewModel> GetAdminPanelViewModel(Guid userId)
        {
            var isAdmin = await roleService.UserIsInRoleAsync(userId.ToString(), "Admin");
            if (!isAdmin)
            {
                throw new InvalidOperationException("You are not admin!");
            }
            var result = new AdminPanelViewModel();
            var posts = await repo.All<Post>().ToListAsync();
            var users = await repo.All<ApplicationUser>().Where(u => u.Id != userId).ToListAsync();
            var schools = await repo.All<School>().Select(s => new SchoolManageViewModel
            {
                Description = s.Description,
                Id = s.Id,
                ImageUrl = s.ImageUrl,
                Location = s.Location,
                Name = s.Name,
                PrincipalId = s.PrincipalId,
                ImageFile = null,
                Principal = s.Principal,
                Parents = new List<ApplicationUser>(),
                Students = new List<ApplicationUser>(),
                Teachers = new List<ApplicationUser>(),
                Posts = new List<PostViewModel>(),
            }).ToListAsync();

            schools = schools.DistinctBy(s => s.Id).ToList();

            foreach (var school in schools)
            {
                var schoolUsers = users.Where(u => u.SchoolId == school.Id).ToList();
                var schoolPosts = posts.Where(p => p.SchoolId == school.Id).ToList();
                foreach (var user in schoolUsers)
                {
                    if (user.IsParent)
                    {
                        school.Parents.Add(user);
                    }
                    else if (user.IsStudent)
                    {
                        school.Students.Add(user);
                    }
                    else
                    {
                        if (!user.IsPrincipal)
                        {
                            school.Teachers.Add(user);
                        }
                    }
                }
            }

            foreach (var user in users)
            {
                user.Posts = posts.Where(p => p.CreatorId == user.Id).ToList();
            }

            result.Schools = schools;
            result.Users = users;
            return result;
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


        public async Task UpdateAsync(Guid userId, UserManageViewModel model)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                throw new ArgumentException("User doesn't exist");
            }

            try
            {
                // Get the uploaded file from the request
                var file = model.ImageFile;
                var fileExtension = Path.GetExtension(file?.FileName);

                // Check if a file was uploaded
                if ((file is not null || file?.Length != 0) && (fileExtension == ".jpg" || fileExtension == ".png"))
                {


                    // Create a unique file name to save the uploaded image
                    var fileName = Guid.NewGuid().ToString() + fileExtension;

                    // Specify the directory where the image will be saved
                    var imagePath = Path.Combine(env.WebRootPath, "images", "user-images", fileName);

                    // Save the file to the specified path
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await file!.CopyToAsync(stream);
                    }

                    model.ImageUrl = $"images/user-images/{fileName}";
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
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
