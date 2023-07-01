using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Common;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models;

namespace SchoolSocialMediaApp.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository repo;

        public PostService(IRepository _repo)
        {
            this.repo = _repo;
        }

        public async Task<IEnumerable<PostViewModel>> GetAllPostsAsync()
        {
            var posts = await repo.All<Post>().Where(p=>p.);
        }
    }
}
