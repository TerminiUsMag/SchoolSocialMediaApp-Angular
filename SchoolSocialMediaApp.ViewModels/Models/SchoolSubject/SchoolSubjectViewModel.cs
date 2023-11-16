using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.ClassesAndSubjects;
using SchoolSocialMediaApp.ViewModels.Models.School;
using SchoolSocialMediaApp.ViewModels.Models.SchoolClass;
using System.ComponentModel.DataAnnotations;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;

namespace SchoolSocialMediaApp.ViewModels.Models.SchoolSubject
{
    [Comment("A basic school subject view model")]
    public class SchoolSubjectViewModel
    {
        [Comment("The unique identifier of the subject")]
        [Key]
        public Guid Id { get; set; }

        [Comment("Name of the subject")]
        [Required]
        [StringLength(validation.MaxSchoolSubjectNameLength,MinimumLength = validation.MinSchoolSubjectNameLength, ErrorMessage = validation.InvalidSchoolSubjectName)]
        public string Name { get; set; } = null!;

        [Comment("The teacher of the subject")]
        public ApplicationUser? Teacher { get; set; }

        [Comment("The id of the teacher")]
        public Guid? TeacherId { get; set; }

        [Comment("The school which the subject is part of")]
        [Required]
        public SchoolViewModel School { get; set; } = null!;

        [Comment("The school's id")]
        [Required]
        public Guid SchoolId { get; set; }

        [Comment("The date and time the subject was created")]
        [Required]
        public DateTime CreatedOn { get; set; }

        public ICollection<SchoolClassViewModel> Classes { get; set; } = new List<SchoolClassViewModel>();
    }
}
