using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;
namespace SchoolSocialMediaApp.ViewModels.Models.User
{
    [Comment("Model for registering a user")]
    public class RegisterViewModel
    {
        [Comment("The first name of the user.")]
        [Required(ErrorMessage = validation.RequiredFirstNameRegisterViewModel)]
        [StringLength(validation.MaxFirstNameLength, MinimumLength = validation.MinFirstNameLength, ErrorMessage = validation.InvalidFirstName)]
        public string FirstName { get; set; } = null!;

        [Comment("The last name of the user.")]
        [Required(ErrorMessage = validation.RequiredLastNameRegisterViewModel)]
        [StringLength(validation.MaxLastNameLength, MinimumLength = validation.MinLastNameLength, ErrorMessage = validation.InvalidLastName)]
        public string LastName { get; set; } = null!;

        [Comment("The email address of the user.")]
        [Required(ErrorMessage = validation.RequiredEmailRegisterViewModel)]
        [StringLength(validation.MaxEmailLength, MinimumLength = validation.MinEmailLength, ErrorMessage = validation.InvalidEmail)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Comment("The password of the user.")]
        [Required(ErrorMessage = validation.RequiredPasswordRegisterViewModel)]
        [StringLength(validation.MaxPasswordLength, MinimumLength = validation.MinPasswordLength, ErrorMessage = validation.InvalidPassword)]
        [DataType(DataType.Password)]
        [Compare(nameof(ConfirmPassword), ErrorMessage = validation.PasswordsDoNotMatch)]
        public string Password { get; set; } = null!;

        [Comment("Repeat password of the user.")]
        [Required(ErrorMessage = validation.RequiredConfirmPasswordRegisterViewModel)]
        [StringLength(validation.MaxPasswordLength, MinimumLength = validation.MinPasswordLength, ErrorMessage = validation.InvalidPassword)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;

        [Comment("The phone number of the user.")]
        [Required(ErrorMessage = validation.RequiredPhoneNumberRegisterViewModel)]
        [StringLength(validation.MaxPhoneNumberLength, MinimumLength = validation.MinPhoneNumberLength, ErrorMessage = validation.InvalidPhoneNumber)]
        //[RegularExpression(validation.PhoneNumberRegEx, ErrorMessage = validation.InvalidPhoneNumber)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null!;
    }
}
