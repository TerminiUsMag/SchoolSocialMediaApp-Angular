using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.ViewModels.Models.School;
using SchoolSocialMediaApp.ViewModels.Models.SchoolClass;
using SchoolSocialMediaApp.ViewModels.Models.SchoolSubject;

namespace SchoolSocialMediaApp.ViewModels.Models.Teacher
{
    /// <summary>
    /// View Model for the teacher panel
    /// </summary>
    public class TeacherPanelViewModel
    {
        [Comment("A collection of all school subjects the teacher is teaching.")]
        public List<SchoolSubjectViewModel> SchoolSubjects { get; set; } = new List<SchoolSubjectViewModel> ();

        [Comment("A collection of all classes the teacher is teaching.")]
        public List<SchoolClassViewModel> SchoolClasses { get; set; } = new List<SchoolClassViewModel>();

        [Comment("The school in which the teacher is teaching currently.")]
        public SchoolViewModel School { get; set; } = null!;


    }
}
