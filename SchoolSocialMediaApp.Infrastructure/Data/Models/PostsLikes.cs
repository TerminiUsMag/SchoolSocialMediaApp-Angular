using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    [Comment("Users who have liked a post")]
    public class PostsLikes
    {
        [Comment("The unique identifier for the post which is liked.")]
        public Guid PostId { get; set; }

        [Comment("Post which is liked")]
        [ForeignKey(nameof(PostId))]
        [Required]
        public Post Post { get; set; } = null!;

        [Comment("The unique identifier for the user who liked the post.")]
        public Guid UserId { get; set; }

        [Comment("User who liked the post")]
        [ForeignKey(nameof(UserId))]
        [Required]
        public ApplicationUser User { get; set; } = null!;
    }
}
