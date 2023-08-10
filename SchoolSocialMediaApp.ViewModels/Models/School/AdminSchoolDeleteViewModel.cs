using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;

namespace SchoolSocialMediaApp.ViewModels.Models.School
{
    public class AdminSchoolDeleteViewModel
    {
        [Comment("The id of the school.")]
        [Required]
        public Guid Id { get; set; }

        [Comment("The name of the school.")]
        [StringLength(validation.MaxSchoolNameLength, MinimumLength = validation.MinSchoolNameLength)]
        [Required]
        public string Name { get; set; } = null!;

        [Comment("The description of the school.")]
        [StringLength(validation.MaxSchoolDescriptionLength, MinimumLength = validation.MinSchoolDescriptionLength)]
        [Required]
        public string Description { get; set; } = null!;

        [Comment("The password of the user.")]
        [Required(ErrorMessage = validation.RequiredPasswordDeleteSchoolViewModel)]
        [StringLength(validation.MaxPasswordLength, MinimumLength = validation.MinPasswordLength, ErrorMessage = validation.InvalidPassword)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

    }
}

