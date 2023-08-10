using SchoolSocialMediaApp.ViewModels.Models.Comment;
using SchoolSocialMediaApp.ViewModels.Models.Post;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface IPostService
    {

        /// <summary>
        /// Add comment to a post.
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="userId"></param>
        /// <param name="commentText"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task AddCommentAsync(Guid postId, Guid userId, string commentText);

        /// <summary>
        /// Creates a post, checks if its valid and adds it to the DB.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task<bool> CreatePostAsync(PostCreateModel model, Guid userId);

        /// <summary>
        /// Delete a comment of a post.
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task DeleteCommentAsync(Guid commentId);

        /// <summary>
        /// Delete a post and all it's comments.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task<Guid> DeletePostAsync(Guid id, Guid userId);

        /// <summary>
        /// Edits a post.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task<Guid> EditPostAsync(PostEditViewModel model, Guid userId);

        /// <summary>
        /// Gets all posts of a school and maps them to PostViewModel.
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ICollection<PostViewModel>> GetAllPostsAsync(Guid schoolId, Guid userId);

        /// <summary>
        /// Get all comments of a post.
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task<List<CommentViewModel>> GetCommentsByPostIdAsync(Guid postId);

        /// <summary>
        /// Gets a post by it's ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task<PostViewModel> GetPostByIdAsync(Guid id);

        /// <summary>
        /// Like a post.
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task LikePostAsync(Guid postId, Guid userId);

        /// <summary>
        /// Unlike a post.
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task UnlikePostAsync(Guid postId, Guid userId);
    }
}
