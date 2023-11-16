using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    [Comment("School Classes and their School Subjects")]
    public class ClassesAndSubjects
    {
        [Comment("School Class")]
        [Required]
        [ForeignKey(nameof(SchoolClassId))]
        public SchoolClass SchoolClass { get; set; } = null!;

        [Comment("School Class id")]
        public Guid SchoolClassId { get; set; }

        [Comment("School Subject")]
        [Required]
        [ForeignKey(nameof(SchoolSubjectId))]
        public SchoolSubject SchoolSubject { get; set; } = null!;

        [Comment("School Subject id")]
        public Guid SchoolSubjectId { get; set; }


    }
}