using SchoolSocialMediaApp.ViewModels.Models.Comment;
using SchoolSocialMediaApp.ViewModels.Models.Post;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface IPostService
    {
        Task AddCommentAsync(Guid postId, Guid userId, string commentText);
        Task<bool> CreatePostAsync(PostCreateModel model, Guid userId);
        Task DeleteCommentAsync(Guid commentId);
        Task DeletePostAsync(Guid id, Guid userId);
        Task EditPostAsync(PostEditViewModel model, Guid userId);
        Task<ICollection<PostViewModel>> GetAllPostsAsync(Guid schoolId,Guid userId);
        Task<List<CommentViewModel>> GetCommentsByPostIdAsync(Guid postId);
        Task<PostViewModel> GetPostByIdAsync(Guid id);
        Task LikePostAsync(Guid postId, Guid userId);
        Task UnlikePostAsync(Guid postId, Guid userId);
    }
}
