using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;

namespace SchoolSocialMediaApp.ViewModels.Models.SchoolSubject
{
    [Comment("A model for creating a school subject")]
    public class SchoolSubjectCreateModel
    {
        [Comment("Name of the subject")]
        [StringLength(validation.MaxSchoolSubjectNameLength, MinimumLength = validation.MinSchoolSubjectNameLength, ErrorMessage = validation.InvalidSchoolSubjectName)]
        public string Name { get; set; } = null!;

        [Comment("Teacher of the subject")]
        public ApplicationUser? Teacher { get; set; }

        [Comment("Teacher's id")]
        public Guid TeacherId { get; set; }

        [Comment("School's id")]
        public Guid SchoolId { get; set; }

        [Comment("A list of candidate teachers to teach the subject you're creating")]
        public ICollection<ApplicationUser> CandidateTeachers { get; set; } = new List<ApplicationUser>();
    }
}
