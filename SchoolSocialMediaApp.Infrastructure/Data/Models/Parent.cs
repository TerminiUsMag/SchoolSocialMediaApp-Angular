using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    [Comment("A parent that has a student in a school.")]
    public class Parent
    {
        [Comment("The id of the parent.")]
        [Key]
        public Guid Id { get; set; }

        [Comment("The id of the user that is the parent.")]
        [Required]
        public Guid UserId { get; set; }

        [Comment("The user that is the parent.")]
        [Required]
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        [Comment("Students of the parent.")]
        [Required]
        public IEnumerable<StudentsParents> Students { get; set; } = null!;

        [Comment("The id of the school that the student is in.")]
        [Required]
        public Guid SchoolId { get; set; }

        [Comment("The school that the student is in.")]
        [Required]
        [ForeignKey(nameof(SchoolId))]
        public School School { get; set; } = null!;
    }
}