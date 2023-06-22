using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    [Comment("Comments which are disliked by users")]
    public class CommentsDislikes
    {
        [Comment("The unique identifier for the user who disliked the comment.")]
        public Guid UserId { get; set; }

        [Comment("User who disliked the comment")]
        [ForeignKey(nameof(UserId))]
        [Required]
        public ApplicationUser User { get; set; } = null!;

        [Comment("The unique identifier for the comment which is disliked.")]
        public Guid CommentId { get; set; }

        [Comment("Comment which is disliked")]
        [ForeignKey(nameof(CommentId))]
        [Required]
        public Comment Comment { get; set; } = null!;
    }
}
