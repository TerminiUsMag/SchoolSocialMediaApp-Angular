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

        [Comment("The id of the student that the parent is a parent of.")]
        [Required]
        public Guid StudentId { get; set; }

        [Comment("The student that the parent is a parent of.")]
        [Required]
        [ForeignKey(nameof(StudentId))]
        public IEnumerable<Student> Students { get; set; } = null!;

        [Comment("The id of the school that the student is in.")]
        [Required]
        public Guid SchoolId { get; set; }

        [Comment("The school that the student is in.")]
        [Required]
        [ForeignKey(nameof(SchoolId))]
        public School School { get; set; } = null!;
    }
}