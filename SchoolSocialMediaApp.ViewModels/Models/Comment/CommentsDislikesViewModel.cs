using SchoolSocialMediaApp.ViewModels.Models.User;

namespace SchoolSocialMediaApp.ViewModels.Models.Comment
{
    public class CommentsDislikesViewModel
    {

        public Guid CommentId { get; set; }

        public CommentViewModel Comment { get; set; } = null!;

        public Guid DislikerId { get; set; }

        public UserViewModel Disliker { get; set; } = null!;
    }
}