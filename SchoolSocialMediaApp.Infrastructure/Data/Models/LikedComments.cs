using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    [Comment("Comments liked by user")]
    public class LikedComments
    {
        [Comment("The unique identifier for the user who liked the comment.")]
        public Guid UserId { get; set; }

        [Comment("User who liked the comment")]
        [ForeignKey(nameof(UserId))]
        [Required]
        public ApplicationUser User { get; set; } = null!;

        [Comment("The unique identifier for the comment which is liked.")]
        public Guid CommentId { get; set; }

        [Comment("Comment which is liked")]
        [ForeignKey(nameof(CommentId))]
        [Required]
        public Comment Comment { get; set; } = null!;
    }
}
