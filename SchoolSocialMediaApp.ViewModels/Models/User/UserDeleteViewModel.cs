using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;

namespace SchoolSocialMediaApp.ViewModels.Models.User
{
    public class UserDeleteViewModel
    {
        [Comment("The password of the user.")]
        [Required(ErrorMessage = validation.RequiredPasswordDeleteViewModel)]
        [StringLength(validation.MaxPasswordLength, MinimumLength = validation.MinPasswordLength, ErrorMessage = validation.InvalidPassword)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
