using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;

namespace SchoolSocialMediaApp.ViewModels.Models
{
    [Comment("A school view model for the school")]
    public class SchoolViewModel
    {
        [Comment("The id of the school.")]
        public Guid Id { get; set; }

        [Comment("The name of the school.")]
        [StringLength(validation.MaxSchoolNameLength, MinimumLength = validation.MinSchoolNameLength)]
        [Required]
        public string Name { get; set; } = null!;

        [Comment("The description of the school.")]
        [StringLength(validation.MaxSchoolDescriptionLength, MinimumLength = validation.MinSchoolDescriptionLength)]
        [Required]
        public string Description { get; set; } = null!;

        [Comment("The image url of the school.")]
        //[StringLength(validation.MaxImageUrlLength, MinimumLength = validation.MinImageUrlLength)]
        //[Required]
        public string? ImageUrl { get; set; }

        [Comment("The location of the school.")]
        [StringLength(validation.MaxSchoolLocationLength, MinimumLength = validation.MinSchoolLocationLength)]
        [Required]
        public string Location { get; set; } = null!;

        [Comment("The id of the director of the school.")]
        public Guid PrincipalId { get; set; }

        [Comment("The name of the director of the school.")]
        public string? PrincipalName { get; set; }
    }
}
