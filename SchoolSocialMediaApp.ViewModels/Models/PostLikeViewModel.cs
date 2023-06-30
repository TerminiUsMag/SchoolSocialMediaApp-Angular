namespace SchoolSocialMediaApp.ViewModels.Models
{
    public class PostLikeViewModel
    {
        public Guid PostId { get; set; }

        public PostViewModel Post { get; set; } = null!;

        public Guid LikerId { get; set; }

        public UserViewModel Liker { get; set; } = null!;
    }
}