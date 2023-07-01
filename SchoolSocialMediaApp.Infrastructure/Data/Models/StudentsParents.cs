using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    [Comment("Parents to students mapping table")]
    public class StudentsParents
    {
        [Comment("The id of the student.")]
        [Required]
        public Guid StudentId { get; set; }

        [Comment("The student.")]
        [Required]
        public Student Student { get; set; } = null!;

        [Comment("The id of the parent.")]
        [Required]
        public Guid ParentId { get; set; }

        [Comment("The parent.")]
        [Required]
        public Parent Parent { get; set; } = null!;

    }
}
