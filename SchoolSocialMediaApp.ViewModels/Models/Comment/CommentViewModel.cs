using SchoolSocialMediaApp.ViewModels.Models.Post;
using SchoolSocialMediaApp.ViewModels.Models.User;

namespace SchoolSocialMediaApp.ViewModels.Models.Comment
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public Guid CreatorId { get; set; }

        public UserViewModel Creator { get; set; } = null!;

        public Guid PostId { get; set; }

        public PostCreateModel Post { get; set; } = null!;
    }
}