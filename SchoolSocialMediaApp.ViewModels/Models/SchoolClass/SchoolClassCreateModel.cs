using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;

namespace SchoolSocialMediaApp.ViewModels.Models.SchoolClass
{
    public class SchoolClassCreateModel
    {
        [Comment("Name of the school class")]
        [StringLength(validation.MaxSchoolClassNameLength, MinimumLength = validation.MinSchoolClassNameLength, ErrorMessage = "School class name must be between {1} and {2} characters.")]
        public string Name { get; set; } = null!;

        [Comment("Grade of the school class")]
        [Range(validation.MinSchoolClassGrade, validation.MaxSchoolClassGrade, ErrorMessage = "Grade must be an integer number between {1} and {2}")]
        public int Grade { get; set; }
    }
}
