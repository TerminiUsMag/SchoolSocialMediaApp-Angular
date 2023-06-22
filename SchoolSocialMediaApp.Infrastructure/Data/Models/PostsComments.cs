using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    [Comment("Comments on a post")]
    public class PostsComments
    {
        [Comment("The unique identifier for the post which is commented on.")]
        public Guid PostId { get; set; }

        [Comment("Post which is commented on")]
        [ForeignKey(nameof(PostId))]
        [Required]
        public Post Post { get; set; } = null!;

        [Comment("The unique identifier for the comment.")]
        public Guid CommentId { get; set; }

        [Comment("Comment on a post")]
        [ForeignKey(nameof(CommentId))]
        [Required]
        public Comment Comment { get; set; } = null!;
    }
}
