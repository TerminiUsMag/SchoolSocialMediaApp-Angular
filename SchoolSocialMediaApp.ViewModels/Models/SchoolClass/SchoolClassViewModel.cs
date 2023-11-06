using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.School;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;

namespace SchoolSocialMediaApp.ViewModels.Models.SchoolClass
{
    [Comment("A basic view model for a school class")]
    public class SchoolClassViewModel
    {
        [Comment("The unique identifier of the school class")]
        [Key]
        public Guid Id { get; set; }

        [Comment("The name of the school class")]
        [Required]
        [MaxLength(validation.MaxSchoolClassNameLength)]
        [MinLength(validation.MinSchoolClassNameLength)]
        public string Name { get; set; } = null!;

        [Comment("The grade of the school class")]
        [Required]
        [Range(validation.MinSchoolClassGrade, validation.MaxSchoolClassGrade)]
        public int Grade { get; set; }

        [Comment("The school in which the class is part of")]
        [Required]
        public SchoolViewModel School { get; set; } = null!;

        [Comment("The school's id")]
        public Guid SchoolId { get; set; }

        [Comment("The date and time the school class was created")]
        [Required]
        public DateTime CreatedOn { get; set; }

        [Comment("Students in the class")]
        public ICollection<ApplicationUser> Students { get; set; } = new List<ApplicationUser>();

        [Comment("School subjects of the class")]
        public ICollection<SchoolSubject> Subjects { get; set; } = new List<SchoolSubject>();
    }
}
