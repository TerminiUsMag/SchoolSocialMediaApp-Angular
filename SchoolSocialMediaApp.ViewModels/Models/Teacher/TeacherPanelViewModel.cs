using Microsoft.EntityFrameworkCore;
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

        public List<SchoolClassAnd>
    }
}
