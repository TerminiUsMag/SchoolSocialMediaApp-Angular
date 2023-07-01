using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using validation = SchoolSocialMediaApp.Common.InfrastructureCommon.ValidationConstantsInfrastructure;

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
        [MaxLength(validation.MaxPostLength)]
        public string Content { get; set; } = null!;

        [Comment("The date and time the post was created.")]
        [Required]
        public DateTime CreatedOn { get; set; }

        [Comment("Id of the post owner")]
        [Required]
        public Guid CreatorId { get; set; }

        [Comment("The post owner")]
        [Required]
        [ForeignKey(nameof(CreatorId))]
        public ApplicationUser Creator { get; set; } = null!;

        [Comment("The id of the school the post is for.")]
        [Required]
        public Guid SchoolId { get; set; }

        [Comment("The school the post is for.")]
        [Required]
        [ForeignKey(nameof(SchoolId))]
        public School School { get; set; } = null!;

        [Comment("The comments on the post.")]
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

        [Comment("The likes on the post.")]
        public IEnumerable<PostsLikes> Likes { get; set; } = new List<PostsLikes>();

        [Comment("The dislikes on the post.")]
        public IEnumerable<PostsDislikes> Dislikes { get; set; } = new List<PostsDislikes>();
    }
}
