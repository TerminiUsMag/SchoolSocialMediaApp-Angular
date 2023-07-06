//using Microsoft.EntityFrameworkCore;
//using System.ComponentModel.DataAnnotations;

//namespace SchoolSocialMediaApp.Infrastructure.Data.Models
//{
//    [Comment("Parents to students mapping table")]
//    public class StudentsParents
//    {
//        [Comment("The id of the student.")]
//        [Required]
//        public Guid StudentId { get; set; }

//        [Comment("The student.")]
//        [Required]
//        public ApplicationUser Student { get; set; } = null!;

//        [Comment("The id of the parent.")]
//        [Required]
//        public Guid ParentId { get; set; }

//        [Comment("The parent.")]
//        [Required]
//        public ApplicationUser Parent { get; set; } = null!;

//    }
//}
