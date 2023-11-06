using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    [Comment("A school subject with a teacher which teaches the subject to students")]
    public class SchoolSubject
    {
        [Comment("The unique identifier of the subject")]
        [Key]
        public Guid Id { get; set; }

        [Comment("Name of the subject")]
        [Required]
        [MaxLength(validation.MaxSubjectNameLength)]
        public string Name { get; set; } = null!;

        [Comment("The teacher of the subject")]
        [ForeignKey(nameof(TeacherId))]
        public ApplicationUser? Teacher { get; set; }

        [Comment("The id of the teacher")]
        public Guid? TeacherId { get; set; }

        [Comment("The school which the subject is part of")]
        [Required]
        [ForeignKey(nameof(SchoolId))]
        public School School { get; set; } = null!;

        [Comment("The school's id")]
        public Guid SchoolId { get; set; }

        [Comment("The date and time the subject was created")]
        [Required]
        public DateTime CreatedOn { get; set; }

    }
}
