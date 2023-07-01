using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using validation = SchoolSocialMediaApp.Common.InfrastructureCommon.ValidationConstantsInfrastructure;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Comment("The first name of the user.")]
        [MaxLength(validation.MaxFirstNameLength)]
        [Required]
        public string FirstName { get; set; } = null!;

        [Comment("The last name of the user.")]
        [MaxLength(validation.MaxLastNameLength)]
        [Required]
        public string LastName { get; set; } = null!;

        [Comment("The image url of the user.")]
        [MaxLength(validation.MaxImageUrlLength)]
        [Required]
        public string ImageUrl { get; set; } = null!;

        [Comment("The date and time the user was created.")]
        [Required]
        public DateTime CreatedOn{ get; set; }

        [Comment("The posts the user has liked.")]
        public IEnumerable<PostsLikes> LikedPosts { get; set; } = new List<PostsLikes>();

        [Comment("The posts the user has disliked.")]
        public IEnumerable<PostsDislikes> DislikedPosts { get; set; } = new List<PostsDislikes>();

        [Comment("The comments the user has liked.")]
        public IEnumerable<CommentsLikes> LikedComments { get; set; } = new List<CommentsLikes>();

        [Comment("The comments the user has disliked.")]
        public IEnumerable<CommentsDislikes> DislikedComments { get; set; } = new List<CommentsDislikes>();

        [Comment("The posts made by the user.")]
        public IEnumerable<Post> Posts { get; set; } = new List<Post>();

        [Comment("The comments made by the user.")]
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
    }
}
