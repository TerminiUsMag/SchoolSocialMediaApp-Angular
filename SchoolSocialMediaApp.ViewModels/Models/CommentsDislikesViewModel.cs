namespace SchoolSocialMediaApp.ViewModels.Models
{
    public class CommentsDislikesViewModel
    {

        public Guid CommentId { get; set; }

        public CommentViewModel Comment { get; set; } = null!;

        public Guid DislikerId { get; set; }

        public UserViewModel Disliker { get; set; } = null!;
    }
}