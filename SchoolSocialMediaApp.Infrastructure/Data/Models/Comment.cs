using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using validation = SchoolSocialMediaApp.Common.InfrastructureCommon.ValidationConstantsInfrastructure;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    [Comment("A comment made by a user on a post.")]
    public class Comment
    {
        [Comment("The unique identifier for the comment.")]
        [Key]
        public Guid Id { get; set; }

        [Comment("The content of the comment.")]
        [Required]
        [MaxLength(validation.MaxCommentLength)]
        public string Content { get; set; } = null!;

        [Comment("The date and time the comment was created.")]
        [Required]
        public DateTime CreatedOn { get; set; }

        [Comment("Id of the post the comment is on.")]
        [Required]
        public Guid PostId { get; set; }

        [Comment("The post the comment is on.")]
        [Required]
        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; } = null!;

        [Comment("Id of the creator of the comment.")]
        [Required]
        public Guid CreatorId { get; set; }

        [Comment("The creator of the comment.")]
        [Required]
        [ForeignKey(nameof(CreatorId))]
        public ApplicationUser Creator { get; set; } = null!;

        [Comment("Likes of the comment.")]
        public IEnumerable<CommentsLikes> Likes { get; set; } = new List<CommentsLikes>();

        [Comment("Dislikes of the comment.")]
        public IEnumerable<CommentsDislikes> Dislikes { get; set; } = new List<CommentsDislikes>();
    }
}