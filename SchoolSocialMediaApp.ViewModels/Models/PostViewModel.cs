namespace SchoolSocialMediaApp.ViewModels.Models
{
    public class PostViewModel
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public Guid CreatorId { get; set; }

        public UserViewModel Creator { get; set; } = null!;

        public IEnumerable<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();

        public IEnumerable<PostLikeViewModel> Likes { get; set; } = new List<PostLikeViewModel>();

        public IEnumerable<PostDislikeViewModel> Dislikes { get; set; } = new List<PostDislikeViewModel>();

    }
}
