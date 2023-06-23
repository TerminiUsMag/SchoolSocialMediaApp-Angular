using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;

namespace SchoolSocialMediaApp.Models
{
    public class LoginViewModel
    {
        [Comment("Email Address")]
        [StringLength(validation.MaxEmailLength, MinimumLength = validation.MinEmailLength, ErrorMessage = validation.InvalidEmail)]
        [Required(ErrorMessage = validation.RequiredEmailLoginViewModel)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Comment("Password")]
        [StringLength(validation.MaxPasswordLength, MinimumLength = validation.MinPasswordLength, ErrorMessage = validation.InvalidPassword)]
        [Required(ErrorMessage = validation.RequiredPasswordLoginViewModel)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Comment("Return Url")]
        public string? ReturnUrl { get; set; }

        [Comment("Remember Me")]
        public bool RememberMe { get; set; }
    }
}
