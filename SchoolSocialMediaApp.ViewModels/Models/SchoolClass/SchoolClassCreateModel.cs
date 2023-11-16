using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;

namespace SchoolSocialMediaApp.ViewModels.Models.SchoolClass
{
    public class SchoolClassCreateModel
    {
        [Comment("Name of the school class")]
        [StringLength(validation.MaxSchoolClassNameLength, MinimumLength = validation.MinSchoolClassNameLength, ErrorMessage = validation.InvalidSchoolClassName)]
        public string Name { get; set; } = null!;

        [Comment("Grade of the school class")]
        [Range(validation.MinSchoolClassGrade, validation.MaxSchoolClassGrade, ErrorMessage = validation.InvalidSchoolClassGrade)]
        public int Grade { get; set; }
    }
}
