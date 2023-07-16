using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;

namespace SchoolSocialMediaApp.ViewModels.Models.User
{
    public class UserManageViewModel
    {
        [Comment("The Id of the user")]
        [Required(ErrorMessage = validation.RequiredId)]
        public Guid Id { get; set; }

        [Comment("The First Name of the user")]
        [StringLength(validation.MaxFirstNameLength, MinimumLength = validation.MinFirstNameLength, ErrorMessage = validation.InvalidFirstName)]
        [Required(ErrorMessage = validation.RequiredFirstName)]
        public string FirstName { get; set; } = null!;

        [Comment("The Last Name of the user")]
        [StringLength(validation.MaxLastNameLength, MinimumLength = validation.MinLastNameLength, ErrorMessage = validation.InvalidLastName)]
        [Required(ErrorMessage = validation.RequiredLastName)]
        public string LastName { get; set; } = null!;

        [Comment("The Username of the user")]
        [StringLength(validation.MaxUsernameLength, MinimumLength = validation.MinUsernameLength, ErrorMessage = validation.InvalidUsername)]
        [Required(ErrorMessage = validation.RequiredUsername)]
        public string Username { get; set; } = null!;

        [Comment("The Image Url of the user")]
        [Required(ErrorMessage = validation.RequiredImageUrl)]
        public string ImageUrl { get; set; } = null!;

        [Comment("The Image File of the user")]
        public IFormFile? ImageFile { get; set; }

        [Comment("Email Address")]
        [StringLength(validation.MaxEmailLength, MinimumLength = validation.MinEmailLength, ErrorMessage = validation.InvalidEmail)]
        [Required(ErrorMessage = validation.RequiredEmailLoginViewModel)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Comment("The Phone Number of the user")]
        [StringLength(validation.MaxPhoneNumberLength, MinimumLength = validation.MinPhoneNumberLength, ErrorMessage = validation.InvalidPhoneNumber)]
        [Required(ErrorMessage = validation.RequiredPhoneNumber)]
        public string PhoneNumber { get; set; } = null!;
    }
}
