using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Common;
using System.ComponentModel.DataAnnotations;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;
namespace SchoolSocialMediaApp.Models
{
    [Comment("Model for registering a user")]
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(validation.MaxFirstNameLength, MinimumLength = validation.MinFirstNameLength, ErrorMessage = validation.InvalidFirstName)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(validation.MaxLastNameLength, MinimumLength = validation.MinLastNameLength, ErrorMessage = validation.InvalidLastName)]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email address is required")]
        [StringLength(validation.MaxEmailLength, MinimumLength = validation.MinEmailLength, ErrorMessage = validation.InvalidEmail)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(validation.MaxPasswordLength, MinimumLength = validation.MinPasswordLength, ErrorMessage = validation.InvalidPassword)]
        [DataType(DataType.Password)]
        [Compare(nameof(ConfirmPassword), ErrorMessage = validation.PasswordsDoNotMatch)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(validation.MaxPasswordLength, MinimumLength = validation.MinPasswordLength, ErrorMessage = validation.InvalidPassword)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Phone Number is required")]
        [StringLength(validation.MaxPhoneNumberLength, MinimumLength = validation.MinPhoneNumberLength,ErrorMessage = validation.InvalidPhoneNumber)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null!;
    }
}
