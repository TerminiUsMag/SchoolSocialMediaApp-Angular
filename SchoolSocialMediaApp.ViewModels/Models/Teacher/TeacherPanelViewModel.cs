using SchoolSocialMediaApp.ViewModels.Models.SchoolClass;
using SchoolSocialMediaApp.ViewModels.Models.SchoolSubject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSocialMediaApp.ViewModels.Models.Teacher
{
    public class TeacherPanelViewModel
    {
        public List<SchoolSubjectViewModel> SchoolSubjects { get; set; } = new List<SchoolSubjectViewModel> ();

        public List<SchoolClassViewModel> SchoolClasses { get; set; } = new List<SchoolClassViewModel>();
    }
}
