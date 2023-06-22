using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    [Comment("Users who have disliked a post")]
    public class PostsDislikes
    {
        [Comment("The unique identifier for the post which is disliked.")]
        public Guid PostId { get; set; }

        [Comment("Post which is disliked")]
        [ForeignKey(nameof(PostId))]
        [Required]
        public Post Post { get; set; } = null!;

        [Comment("The unique identifier for the user who disliked the post.")]
        public Guid UserId { get; set; }

        [Comment("User who disliked the post")]
        [ForeignKey(nameof(UserId))]
        [Required]
        public ApplicationUser User { get; set; } = null!;
    }
}
