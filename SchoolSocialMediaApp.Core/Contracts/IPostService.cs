using SchoolSocialMediaApp.ViewModels.Models.Post;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface IPostService
    {
        Task AddCommentAsync(Guid postId, Guid userId, string commentText);
        Task<bool> CreatePostAsync(PostCreateModel model, Guid userId);
        Task DeletePostAsync(Guid id, Guid userId);
        Task EditPostAsync(PostEditViewModel model, Guid userId);
        Task<ICollection<PostViewModel>> GetAllPostsAsync(Guid schoolId);
        Task<PostViewModel> GetPostByIdAsync(Guid id);
        Task LikePostAsync(Guid postId, Guid userId);
        Task UnlikePostAsync(Guid postId, Guid userId);
    }
}
