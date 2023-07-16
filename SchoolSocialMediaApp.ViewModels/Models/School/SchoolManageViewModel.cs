using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;
namespace SchoolSocialMediaApp.ViewModels.Models.School
{
    public class SchoolManageViewModel
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
        [Required]
        public string ImageUrl { get; set; } = null!;

        [Comment("The location of the school.")]
        [StringLength(validation.MaxSchoolLocationLength, MinimumLength = validation.MinSchoolLocationLength)]
        [Required]
        public string Location { get; set; } = null!;

        [Comment("The id of the director of the school.")]
        [Required]
        public Guid PrincipalId { get; set; }

        [Comment("The principal of the school.")]
        [Required]
        public ApplicationUser Principal { get; set; } = null!;

        [Comment("Parents in the school.")]
        public ICollection<ApplicationUser> Parents { get; set; } = new List<ApplicationUser>();

        [Comment("Students in the school.")]
        public ICollection<ApplicationUser> Students { get; set; } = new List<ApplicationUser>();

        [Comment("Teachers in the school.")]
        public ICollection<ApplicationUser> Teachers { get; set; } = new List<ApplicationUser>();
    }
}
