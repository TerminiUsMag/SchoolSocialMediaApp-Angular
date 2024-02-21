using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    [Comment("A school that has a director and students.")]
    public class School
    {
        [Comment("The id of the school.")]
        [Key]
        public Guid Id { get; set; }

        [Comment("The name of the school.")]
        [MaxLength(validation.MaxSchoolNameLength)]
        [Required]
        public string Name { get; set; } = null!;

        [Comment("The description of the school.")]
        [MaxLength(validation.MaxSchoolDescriptionLength)]
        [Required]
        public string Description { get; set; } = null!;

        [Comment("The image url of the school.")]
        [MaxLength(validation.MaxImageUrlLength)]
        [Required]
        public string ImageUrl { get; set; } = null!;

        [Comment("The location of the school.")]
        [MaxLength(validation.MaxSchoolLocationLength)]
        [Required]
        public string Location { get; set; } = null!;

        [Comment("The id of the principal of the school.")]
        [Required]
        public Guid PrincipalId { get; set; }

        [Comment("The principal of the school.")]
        [Required]
        [ForeignKey(nameof(PrincipalId))]
        public ApplicationUser Principal { get; set; } = null!;

        [Comment("Participants of the school (Parents, Students and Teachers)")]
        public ICollection<ApplicationUser> Participants { get; set; } = new List<ApplicationUser>();

        [Comment("The posts of the school.")]
        public ICollection<Post> Posts { get; set; } = new List<Post>();

        [Comment("The invitations of the school.")]
        public ICollection<Invitation> Invitations { get; set; } = null!;

    }
}
