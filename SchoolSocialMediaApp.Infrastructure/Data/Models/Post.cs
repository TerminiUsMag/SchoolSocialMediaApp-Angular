using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolSocialMediaApp.Infrastructure.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    [Comment("A post made by a user.")]
    public class Post
    {
        [Comment("The unique identifier for the post.")]
        [Key]
        public Guid Id { get; set; }

        [Comment("The content of the post.")]
        [Required]
        [MaxLength(Constants.MaxPostLength)]
        public string Content { get; set; } = null!;

        [Comment("The date and time the post was created.")]
        [Required]
        public DateTime CreatedOn { get; set; }

        [Comment("Id of the post owner")]
        [Required]
        public Guid OwnerId { get; set; }

        [Comment("The post owner")]
        [Required]
        [ForeignKey(nameof(OwnerId))]
        public ApplicationUser Owner { get; set; } = null!;

        [Comment("The comments on the post.")]
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

        [Comment("The likes on the post.")]
        public IEnumerable<ApplicationUser> Likes { get; set; } = new List<ApplicationUser>();

        [Comment("The dislikes on the post.")]
        public IEnumerable<ApplicationUser> Dislikes { get; set; } = new List<ApplicationUser>();
    }
}
