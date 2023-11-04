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

    }
}
