using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using validation = SchoolSocialMediaApp.Common.InfrastructureCommon.ValidationConstantsInfrastructure;

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

        [Comment("The id of the director of the school.")]
        [Required]
        public Guid PrincipalId { get; set; }

        [Comment("The director of the school.")]
        [Required]
        [ForeignKey(nameof(PrincipalId))]
        public ApplicationUser Principal { get; set; } = null!;

        [Comment("The parents of the school.")]
        public IEnumerable<Parent> Parents { get; set; } = new List<Parent>();

        [Comment("The students of the school.")]
        public IEnumerable<Student> Students { get; set; } = new List<Student>();

        [Comment("The posts of the school.")]
        public IEnumerable<Post> Posts { get; set; } = new List<Post>();

    }
}
