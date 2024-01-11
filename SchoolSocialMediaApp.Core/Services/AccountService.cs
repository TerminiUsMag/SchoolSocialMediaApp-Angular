using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Common;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.Admin;
using SchoolSocialMediaApp.ViewModels.Models.Post;
using SchoolSocialMediaApp.ViewModels.Models.School;
using SchoolSocialMediaApp.ViewModels.Models.Teacher;
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
        private readonly ISchoolService schoolService;
        private readonly ISchoolSubjectService subjectService;
        public AccountService(
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager,
            IRepository _repo,
            IRoleService _roleService,
            IWebHostEnvironment _env,
            ISchoolService _schoolService,
            ISchoolSubjectService _subjectService)
        {
            this.userManager = _userManager;
            this.signInManager = _signInManager;
            this.roleService = _roleService;
            this.repo = _repo;
            this.env = _env;
            this.schoolService = _schoolService;
            this.subjectService = _subjectService;
        }

        public async Task DeleteAsync(Guid userId)
        {
            //Get the user entity from the DB with his posts, comments, likes and invitations.
            var user = await repo.All<ApplicationUser>().Where(u => u.Id == userId).Include(u => u.Posts).Include(u => u.Comments).Include(u => u.LikedPosts).Include(u => u.Invitations).FirstOrDefaultAsync();

            //Checks if the user is not found and throws and exception.
            if (user == null) throw new ArgumentNullException("User is empty");

            //Set variables for user's posts, comments, likes and invitations for readability.
            var posts = user.Posts;
            var comments = user.Comments;
            var postLikes = user.LikedPosts;
            var invitations = user.Invitations;

            //Checks for comments, if there are comments they get deleted.
            if (comments.Any())
            {
                repo.DeleteRange(comments);
            }
            //Checks for likes, if there are likes they get removed.
            if (postLikes.Any())
            {
                repo.DeleteRange(postLikes);
            }
            //Checks for posts, if there are posts they get deleted.
            if (posts.Any())
            {
                repo.DeleteRange(posts);
            }
            //Checks for invitations if there are invitations they get deleted.
            if (invitations.Any())
            {
                repo.DeleteRange(invitations);
            }
            //Saves all changes to the DB.
            await repo.SaveChangesAsync();

            //Checks if the account is linked to one of the demo profile pictures and if not deletes the custom profile picture of the user from the storage to save space.
            if (user.ImageUrl != "/images/user-images/principalProfile.jpg" && user.ImageUrl != "/images/user-images/studentProfile.jpg" && user.ImageUrl != "/images/defaultProfile.png")
            {

                string imageUrl = user.ImageUrl.Substring(1);
                string filePath = Path.Combine(env.WebRootPath, imageUrl);

                // Check if the file exists before attempting to delete
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            //Deletes the user from the DB and saves the changes.
            await userManager.DeleteAsync(user);
            await signInManager.SignOutAsync();
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

        public async Task<bool> EmailIsFree(string email, Guid userId)
        {
            var result = await repo.All<ApplicationUser>().Where(u => u.Id != userId).FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpper());

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
            var users = await repo.All<ApplicationUser>().Where(u => u.Id != userId && u.IsAdmin == false).ToListAsync();
            var admins = await repo.All<ApplicationUser>().Where(u => u.Id != userId && u.IsAdmin == true).ToListAsync();
            var schools = await repo.All<School>().Include(s => s.Principal).Select(s => new SchoolManageViewModel
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
            result.Admins = admins;
            return result;
        }

        public async Task<AdminUserDeletionViewModel> GetAdminUserDeletionViewModelAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User id cannot be empty");
            }
            var user = await repo.All<ApplicationUser>().Where(u => u.Id == userId).FirstOrDefaultAsync();

            if (user is null)
            {
                throw new ArgumentException("No user found with Id: " + userId);
            }

            return new AdminUserDeletionViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                Email = user.Email,
                ImageUrl = user.ImageUrl,
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber,
            };

        }

        public async Task<MakeUserAdminViewModel> GetMakeUserAdminViewModelAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User id cannot be empty");
            }
            var user = await repo.All<ApplicationUser>().Where(u => u.Id == userId).FirstOrDefaultAsync();

            if (user is null)
            {
                throw new ArgumentException("No user found with Id: " + userId);
            }

            return new MakeUserAdminViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                Email = user.Email,
                ImageUrl = user.ImageUrl,
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber,
            };
        }

        public async Task<TeacherPanelViewModel> GetTeacherPanelViewModel(Guid userId)
        {
            var user = await repo.All<ApplicationUser>().Include(u => u.Subjects).FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
                throw new ArgumentException("No such user");

            var userIsTeacher = await roleService.UserIsInRoleAsync(userId.ToString(), "Teacher");
            if (!userIsTeacher)
                throw new ArgumentException("User is not a teacher !");

            var schoolId = user.SchoolId;
            var school = await repo.All<School>().FirstOrDefaultAsync(s => s.Id == schoolId);
            if (school is null)
                throw new ArgumentException("No such school");

            var subjects = user.Subjects;
            if (subjects.Any())
            {
                foreach(var subject in subjects)
                {
                    var schoolClassesInSubject = await subjectService.GetAssignedClasses(subject.Id);
                }
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

            return new UserManageViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ImageUrl = user.ImageUrl,
            };

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

        public async Task MakeAdmin(ApplicationUser user)
        {
            if (user.SchoolId is not null)
            {
                if (user.IsPrincipal)
                {
                    throw new ArgumentException("Principals cannot be admins");
                }
                Guid schoolId = (Guid)user.SchoolId;
                await schoolService.RemoveUserFromSchoolAsync(user.Id, schoolId);
            }

            var roles = await roleService.GetUserRolesAsync(user.Id);
            foreach (var role in roles)
            {
                await roleService.RemoveUserFromRoleAsync(user.Id.ToString(), role);
            }
            await repo.SaveChangesAsync();
            await roleService.AddUserToRoleAsync(user.Id.ToString(), "Admin");
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

        public async Task<bool> PhoneNumberIsFree(string phoneNumber, Guid userId)
        {
            var result = await repo.AllReadonly<ApplicationUser>().Where(u => u.Id != userId).FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);

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

                    model.ImageUrl = $"/images/user-images/{fileName}";
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

        public async Task<bool> UsernameIsFree(string username, Guid userId)
        {
            var result = await repo.AllReadonly<ApplicationUser>().Where(u => u.Id != userId).FirstOrDefaultAsync(x => x.NormalizedUserName == username.ToUpper());

            if (result is null)
            {
                return true;
            }
            return false;
        }
    }
}
