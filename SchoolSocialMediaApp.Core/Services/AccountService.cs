using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Common;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using System.Net.Mail;
using System.Text.RegularExpressions;
using validation = SchoolSocialMediaApp.Common.CoreCommon.ValidationConstantsCore;

namespace SchoolSocialMediaApp.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IRepository repo;
        public AccountService(
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager,
            IRepository _repo)
        {
            this.userManager = _userManager;
            this.signInManager = _signInManager;
            this.repo = _repo;
        }

        public Task<bool> CreateParentAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreatePrincipalAsync(Guid userId)
        {
            if(await repo.AllReadonly<ApplicationUser>().FirstOrDefaultAsync(x => x.Id == userId) is null)
            {
                throw new ArgumentException("User does not exist");
            }
            if(await repo.AllReadonly<Principal>().FirstOrDefaultAsync(x => x.UserId == userId) is not null)
            {
                throw new ArgumentException("User is already a principal");
            }
            var principal = new Principal
            {
                UserId = userId
            };
            await repo.AddAsync(principal);
            await repo.SaveChangesAsync();
            return true;
        }

        public Task<bool> CreateStudentAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateTeacherAsync(Guid userId)
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
                await signInManager.SignInAsync(user, isPersistent: false);
                return true;
            }
            return false;
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
