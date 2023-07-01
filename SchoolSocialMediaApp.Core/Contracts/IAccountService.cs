using Microsoft.AspNetCore.Identity;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface IAccountService
    {
        Task<bool> RegisterAsync(ApplicationUser user, string password);
        Task<bool> LoginAsync(string email, string password, bool rememberMe);
        Task LogoutAsync();
        bool EmailIsValid(string email);
        bool PhoneNumberIsValid(string phoneNumber);
        Task<bool> EmailIsFree(string email);
        Task<bool> PhoneNumberIsFree(string phoneNumber);
        Task<bool> UsernameIsFree(string username);
        Task<bool> CreatePrincipalAsync(Guid userId);
        Task<bool> CreateTeacherAsync(Guid userId);
        Task<bool> CreateStudentAsync(Guid userId);
        Task<bool> CreateParentAsync(Guid userId);

    }
}
