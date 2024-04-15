using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    [Comment("Grade of a student")]
    public class Grade
    {
        [Comment("The unique identifier for the grade.")]
        [Key]
        public Guid Id { get; set; }

        [Comment("The grade itself.")]
        [Required]
        [Range(validation.MinGradeValue, validation.MaxGradeValue, ErrorMessage = "Grade must be between {0} and {1}")]
        public int GradeValue { get; set; }

        [Comment("The date and time the grade was created.")]
        [Required]
        public DateTime CreatedOn { get; set; }

        [Comment("Id of the subject")]
        [Required]
        public Guid SubjectId { get; set; }

        [Comment("The subject in which the grade is on.")]
        [Required]
        [ForeignKey(nameof(SubjectId))]
        public SchoolSubject Subject { get; set; } = null!;

        [Comment("Id of the creator of the grade.")]
        [Required]
        public Guid CreatorId { get; set; }

        [Comment("The creator of the grade.")]
        [Required]
        [ForeignKey(nameof(CreatorId))]
        public ApplicationUser Creator { get; set; } = null!;

        [Comment("Id of the student the grade is assigned to.")]
        [Required]
        public Guid StudentId { get; set; }

        [Comment("The student that the grade is assigned to.")]
        [Required]
        [ForeignKey(nameof(StudentId))]
        public ApplicationUser Student { get; set; } = null!;
    }
}
