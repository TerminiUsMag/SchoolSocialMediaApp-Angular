using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    [Comment("Comments made by a user")]
    public class UsersComments
    {
        [Comment("The unique identifier for the user comment.")]
        public Guid CommentId { get; set; }

        [Comment("Comment made by a user")]
        [ForeignKey(nameof(CommentId))]
        [Required]
        public Comment Comment { get; set; } = null!;

        [Comment("The unique identifier for the user who made the comment.")]
        public Guid UserId { get; set; }

        [Comment("User who made the comment")]
        [ForeignKey(nameof(UserId))]
        [Required]
        public ApplicationUser User { get; set; } = null!;
    }
}
