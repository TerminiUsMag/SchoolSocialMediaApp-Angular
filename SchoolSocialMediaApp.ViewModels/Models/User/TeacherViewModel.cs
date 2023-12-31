using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.ViewModels.Models.SchoolSubject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSocialMediaApp.ViewModels.Models.User
{
    [Comment("A view model used for changing subject's teacher.")]
    public class TeacherViewModel
    {
        [Comment("Teacher's id")]
        [Required]
        public Guid Id { get; set; }

        [Comment("Teacher's First and Last name")]
        [Required]
        public string FullName { get; set; } = null!;

        [Comment("Teacher's school id")]
        public Guid SchoolId { get; set; }

        [Comment("Teacher's subjects at the moment")]
        public List<SchoolSubjectViewModel> Subjects { get; set; } = new List<SchoolSubjectViewModel>();
    }
}