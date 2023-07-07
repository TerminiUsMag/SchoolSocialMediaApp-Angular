using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using validation = SchoolSocialMediaApp.Common.InfrastructureCommon.ValidationConstantsInfrastructure;

namespace SchoolSocialMediaApp.ViewModels.Models
{
    public class PostEditViewModel
    {
        [Comment("Post Id")]
        [Required(ErrorMessage = validation.PostIdRequired)]
        public Guid Id { get; set; }

        [Comment("Post Content")]
        [Required(ErrorMessage = validation.PostContentRequired)]
        [StringLength(validation.MaxPostLength, MinimumLength = validation.MinPostLength, ErrorMessage = validation.PostContentRequired)]
        public string Content { get; set; } = null!;

        [Comment("Likes")]
        public IEnumerable<PostLikesViewModel> Likes { get; set; } = new List<PostLikesViewModel>();

        [Comment("Dislikes")]
        public IEnumerable<PostDislikesViewModel> Dislikes { get; set; } = new List<PostDislikesViewModel>();

        [Comment("Comments")]
        public IEnumerable<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();

        [Comment("Post Likes Count")]
        public int LikesCount { get; set; } = 0;

        [Comment("Post Dislikes Count")]
        public int DislikesCount { get; set; } = 0;


    }
}
