using SchoolSocialMediaApp.ViewModels.Models.User;

namespace SchoolSocialMediaApp.ViewModels.Models.Comment
{
    public class CommentsLikesViewModel
    {
        public Guid CommentId { get; set; }

        public CommentViewModel Comment { get; set; } = null!;

        public Guid LikerId { get; set; }

        public UserViewModel Liker { get; set; } = null!;
    }
}