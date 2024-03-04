using SchoolSocialMediaApp.ViewModels.Models.Post;
using SchoolSocialMediaApp.ViewModels.Models.SchoolClass;
using SchoolSocialMediaApp.ViewModels.Models.SchoolSubject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSocialMediaApp.ViewModels.Models.Student
{
    public class StudentSubjectClassViewModel
    {
        public Guid SubjectId { get; set; }

        public SchoolSubjectViewModel Subject { get; set; } = null!;

        public Guid ClassId { get; set; }

        public SchoolClassViewModel Class { get; set; } = null!;

        public Guid StudentId { get; set; }

        public StudentViewModel Student { get; set; } = null!;

        public List<PostViewModel> Posts { get; set; } = new List<PostViewModel>();

    }
}
