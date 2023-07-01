using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<PostViewModel>> GetAllPostsAsync(Guid schoolId)
        {
            var posts = await repo.All<Post>().Where(p => p.SchoolId == schoolId).Select(p => new PostViewModel
            {
                Comments = p.Comments.Select(c => new CommentViewModel
                {
                    Content = c.Content,
                    Creator = new UserViewModel
                    {
                        Id = c.Creator.Id,
                        ImageUrl = c.Creator.ImageUrl,
                        Username = c.Creator.UserName
                    },
                    CreatedOn = c.CreatedOn
                }),
                Creator = new UserViewModel
                {
                    Id = p.Creator.Id,
                    ImageUrl = p.Creator.ImageUrl,
                    Username = p.Creator.UserName
                },
                CreatedOn = p.CreatedOn,
                Dislikes = p.Dislikes.Select(d => new PostDislikeViewModel
                {
                    DislikerId = d.UserId,
                    PostId = d.PostId
                }),
                Id = p.Id,
                Likes = p.Likes.Select(l => new PostLikeViewModel
                {
                    LikerId = l.UserId,
                    PostId = l.PostId,
                }),
                Content = p.Content,
                CreatorId = p.CreatorId,
            }).ToListAsync();

            return posts;
        }
    }
}
