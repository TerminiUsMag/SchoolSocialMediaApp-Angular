//using Microsoft.EntityFrameworkCore;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace SchoolSocialMediaApp.Infrastructure.Data.Models
//{
//    [Comment("Represents an administrator of the application.")]
//    public class Administrator
//    {
//        [Comment("The id of the administrator.")]
//        [Key]
//        public Guid Id { get; set; }

//        [Comment("The id of the user that is an administrator.")]
//        [Required]
//        public Guid UserId { get; set; }

//        [Comment("The user that is an administrator.")]
//        [Required]
//        [ForeignKey(nameof(UserId))]
//        public ApplicationUser User { get; set; } = null!;
//    }
//}