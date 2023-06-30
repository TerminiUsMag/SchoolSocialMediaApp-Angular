using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using validation = SchoolSocialMediaApp.Common.InfrastructureCommon.ValidationConstantsInfrastructure;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    [Comment("A teacher of a school.")]
    public class Teacher
    {

        [Comment("The id of the teacher.")]
        [Key]
        public Guid Id { get; set; }

        [Comment("The id of the user that is a teacher.")]
        [Required]
        public Guid UserId { get; set; }

        [Comment("The user that is a teacher.")]
        [Required]
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        [Comment("The id of the school the teacher is in.")]
        [Required]
        public Guid SchoolId { get; set; }

        [Comment("The school the teacher is in.")]
        [Required]
        [ForeignKey(nameof(SchoolId))]
        public School School { get; set; } = null!;

        [Comment("The subject the teacher teaches.")]
        [Required]
        [MaxLength(validation.MaxSubjectLength)]
        public string SubjectOfTeaching { get; set; } = null!;


    }
}
