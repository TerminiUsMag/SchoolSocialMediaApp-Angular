using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Data.Models;

namespace SchoolSocialMediaApp.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AccountService(
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager)
        {
            this.userManager = _userManager;
            this.signInManager = _signInManager;
        }

        public Task<bool> LoginAsync(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RegisterAsync(ApplicationUser user, string password)
        {
            var result = await userManager.CreateAsync(user, password);
            if(result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return true;
            }
            return false;
        }
    }
}
