using SchoolSocialMediaApp.ViewModels.Models.SchoolClass;
using SchoolSocialMediaApp.ViewModels.Models.SchoolSubject;

namespace SchoolSocialMediaApp.ViewModels.Models.Student
{
    public class StudentPanelViewModel
    {
        public SchoolClassViewModel? SchoolClass { get; set; }

        public List<StudentViewModel> Classmates { get; set; } = new List<StudentViewModel>();

        public List<SchoolSubjectViewModel> SchoolSubjects { get; set; } = new List<SchoolSubjectViewModel>();
    }
}
