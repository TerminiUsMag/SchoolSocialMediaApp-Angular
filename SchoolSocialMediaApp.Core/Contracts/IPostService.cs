using SchoolSocialMediaApp.ViewModels.Models;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface IPostService
    {
        Task<IEnumerable<PostViewModel>> GetAllPostsAsync();
    }
}
