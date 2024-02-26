using SchoolSocialMediaApp.ViewModels.Models.SchoolClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSocialMediaApp.ViewModels.Models.Student
{
    public class StudentViewModel
    {
        public Guid Id { get; set; }

        public SchoolClassViewModel? Class { get; set; }

        public Guid ClassId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

    }
}
