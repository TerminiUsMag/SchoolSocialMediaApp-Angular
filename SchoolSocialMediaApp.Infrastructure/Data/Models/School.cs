using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    [Comment("A school that has a director and students.")]
    public class School
    {
        [Comment("The id of the school.")]
        [Key]
        public Guid Id { get; set; }

        [Comment("The name of the school.")]
        [Required]
        [MaxLength(SchoolSocialMediaApp.Common.InfrastructureCommon.ValidationConstantsInfrastructure.MaxSchoolNameLength)]
        public string Name { get; set; } = null!;

        [Comment("The id of the director of the school.")]
        [Required]
        public Guid PrincipalId { get; set; }

        [Comment("The director of the school.")]
        [Required]
        [ForeignKey(nameof(PrincipalId))]
        public Principal Principal { get; set; } = null!;

        [Comment("The parents of the school.")]
        public IEnumerable<Parent> Parents { get; set; } = new List<Parent>();

        [Comment("The students of the school.")]
        public IEnumerable<Student> Students { get; set; } = new List<Student>();

        [Comment("The posts of the school.")]
        public IEnumerable<Post> Posts { get; set; } = new List<Post>();

    }
}
