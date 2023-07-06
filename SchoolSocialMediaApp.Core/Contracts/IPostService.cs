using SchoolSocialMediaApp.ViewModels.Models;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface IPostService
    {
        Task<bool> CreatePostAsync(PostCreateModel model, Guid userId);
        Task<IEnumerable<PostViewModel>> GetAllPostsAsync(Guid schoolId);
    }
}
