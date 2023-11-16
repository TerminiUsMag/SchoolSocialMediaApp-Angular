using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    [Comment("A school class with a group of students and subjects")]
    public class SchoolClass
    {
        [Comment("The unique identifier of the school class")]
        [Key]
        public Guid Id { get; set; }

        [Comment("The name of the school class")]
        [Required]
        [MaxLength(validation.MaxSchoolClassNameLength)]
        public string Name { get; set; } = null!;

        [Comment("The grade of the school class")]
        [Required]
        [Range(validation.MinSchoolClassGrade, validation.MaxSchoolClassGrade)]
        public int Grade { get; set; }

        [Comment("The school in which the class is part of")]
        [Required]
        [ForeignKey(nameof(SchoolId))]
        public School School { get; set; } = null!;

        [Comment("The school's id")]
        public Guid SchoolId { get; set; }

        [Comment("Date and time the class was created")]
        [Required]
        public DateTime CreatedOn { get; set; }

        [Comment("Students in the class")]
        public ICollection<ApplicationUser> Students { get; set; } = new List<ApplicationUser>();

        [Comment("School subjects of the class")]
        public ICollection<ClassesAndSubjects> SchoolSubjects { get; set; } = new List<ClassesAndSubjects>();

    }
}
