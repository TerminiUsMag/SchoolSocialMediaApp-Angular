using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    [Comment("A student that has a parent and is in a school.")]
    public class Student
    {
        [Comment("The id of the student.")]
        [Key]
        public Guid Id { get; set; }

        [Comment("The id of the user that is the student.")]
        [Required]
        public Guid UserId { get; set; }

        [Comment("The user that is the student.")]
        [Required]
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        [Comment("The id of the school the student is in.")]
        [Required]
        public Guid SchoolId { get; set; }

        [Comment("The school the student is in.")]
        [Required]
        [ForeignKey(nameof(SchoolId))]
        public School School { get; set; } = null!;

        [Comment("Parents of the student.")]
        [Required]
        public IEnumerable<StudentsParents> Parents { get; set; } = null!;
    }
}
