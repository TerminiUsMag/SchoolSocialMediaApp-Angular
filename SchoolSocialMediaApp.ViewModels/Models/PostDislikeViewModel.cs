namespace SchoolSocialMediaApp.ViewModels.Models
{
    public class PostDislikeViewModel
    {
        public Guid PostId { get; set; }

        public PostViewModel Post { get; set; } = null!;

        public Guid DislikerId { get; set; }

        public UserViewModel Disliker { get; set; } = null!;
    }
}