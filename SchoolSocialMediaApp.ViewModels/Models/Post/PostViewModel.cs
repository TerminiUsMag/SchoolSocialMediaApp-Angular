using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.ViewModels.Models.Comment;
using SchoolSocialMediaApp.ViewModels.Models.School;
using SchoolSocialMediaApp.ViewModels.Models.User;
using System.ComponentModel.DataAnnotations;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;

namespace SchoolSocialMediaApp.ViewModels.Models.Post
{
    public class PostViewModel
    {
        [Comment("Post Id")]
        [Required(ErrorMessage = validation.PostIdRequired)]
        public Guid Id { get; set; }

        [Comment("Post Content")]
        [Required(ErrorMessage = validation.PostContentRequired)]
        [StringLength(validation.MaxPostLength, MinimumLength = validation.MinPostLength, ErrorMessage = validation.PostContentRequired)]
        public string Content { get; set; } = null!;

        [Comment("Date and Time of Creation")]
        [Required(ErrorMessage = validation.PostDateAndTimeRequired)]
        public DateTime CreatedOn { get; set; }

        [Comment("Creator Id")]
        [Required(ErrorMessage = validation.PostCreatorIdRequired)]
        public Guid CreatorId { get; set; }

        [Comment("Creator")]
        [Required(ErrorMessage = validation.PostCreatorRequired)]
        public UserViewModel Creator { get; set; } = null!;

        [Comment("School Id")]
        [Required(ErrorMessage = validation.PostSchoolIdRequired)]
        public Guid SchoolId { get; set; }

        [Comment("School")]
        [Required(ErrorMessage = validation.PostSchoolRequired)]
        public SchoolViewModel School { get; set; } = null!;

        [Comment("Likes")]
        public ICollection<PostLikesViewModel> Likes { get; set; } = new List<PostLikesViewModel>();

        [Comment("Comments")]
        public ICollection<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();

        [Comment("Post Likes Count")]
        public int LikesCount { get; set; } = 0;

        [Comment("Post Dislikes Count")]
        public int DislikesCount { get; set; } = 0;

        [Comment("Is Edited")]
        public bool IsEdited { get; set; }

        [Comment("Is Liked By Current User")]
        public bool IsLikedByCurrentUser { get; set; }

    }
}
