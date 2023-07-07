using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Common;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models;
using validation = SchoolSocialMediaApp.Common.InfrastructureCommon.ValidationConstantsInfrastructure;

namespace SchoolSocialMediaApp.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository repo;
        private readonly ISchoolService schoolService;

        public PostService(IRepository _repo, ISchoolService _schoolService)
        {
            this.repo = _repo;
            this.schoolService = _schoolService;
        }

        public async Task<bool> CreatePostAsync(PostCreateModel model, Guid userId)
        {
            Guid? schoolId = await schoolService.GetSchoolIdByUserIdAsync(userId);

            if (schoolId == Guid.Empty || schoolId is null)
            {
                throw new ArgumentException("User does not have a school.");
            }


            var post = new Post
            {
                Content = model.Content,
                CreatorId = userId,
                SchoolId = schoolId.Value,
                CreatedOn = DateTime.Now,
            };
                this.PostIsValid(post);
                await repo.AddAsync(post);
                await repo.SaveChangesAsync();
                return true;
        }

        private void PostIsValid(Post post)
        {
            if (post is null)
            {
                throw new ArgumentException("Post is null.");
            }
            if (post.Content is null)
            {
                throw new ArgumentException("Post content is null.");
            }
            if (post.Content.Length > validation.MaxPostLength)
            {
                throw new ArgumentException($"Post content is too long. Max length is {validation.MaxPostLength}.");
            }
            if (post.Content.Length < validation.MinPostLength)
            {
                throw new ArgumentException($"Post content is too short. Min length is {validation.MinPostLength}.");
            }
            if (post.CreatorId == Guid.Empty)
            {
                throw new ArgumentException("Post creator id is empty.");
            }
            if (post.SchoolId == Guid.Empty)
            {
                throw new ArgumentException("Post school id is empty.");
            }
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
                Dislikes = p.Dislikes.Select(d => new PostDislikesViewModel
                {
                    DislikerId = d.UserId,
                    PostId = d.PostId
                }),
                Id = p.Id,
                Likes = p.Likes.Select(l => new PostLikesViewModel
                {
                    LikerId = l.UserId,
                    PostId = l.PostId,
                }),
                Content = p.Content,
                CreatorId = p.CreatorId,
            }).ToListAsync();

            return posts;
        }

        public async Task EditPostAsync(PostEditViewModel model, Guid userId)
        {
            var post = await repo.All<Post>().FirstOrDefaultAsync(p => p.Id == model.Id);
            if (post is null)
            {
                throw new ArgumentException("There's no such post to edit");
            }

            var newPost = new Post
            {
                Comments = model.Comments.Select(c => new Comment
                {
                    Content = c.Content,
                    CreatedOn = c.CreatedOn,
                    Id = c.Id,
                    CreatorId = c.CreatorId,
                    Dislikes = c.Dislikes.Select(cdl => new CommentsDislikes
                    {
                        CommentId = cdl.CommentId,
                        UserId = cdl.DislikerId
                    }),
                    Likes = c.Likes.Select(cl => new CommentsLikes
                    {
                        CommentId = cl.CommentId,
                        UserId = cl.LikerId
                    }),
                    PostId = c.PostId,
                }),
                Content = model.Content,
                CreatedOn = post.CreatedOn,
                CreatorId = post.CreatorId,
                IsEdited = true,
                SchoolId = post.SchoolId,
                Likes = model.Likes.Select(l=> new PostsLikes
                {
                    PostId = l.PostId,
                    UserId = l.LikerId
                }),
                Dislikes = model.Dislikes.Select(dl=>new PostsDislikes
                {
                    PostId = dl.PostId,
                    UserId = dl.DislikerId
                }),
                Id = model.Id
            };

            post.Comments = newPost.Comments;
            post.Content = newPost.Content;
            post.Dislikes = newPost.Dislikes;
            post.Likes = newPost.Likes;
            post.Creator = newPost.Creator;
            post.CreatorId = newPost.CreatorId;
            post.CreatedOn = newPost.CreatedOn;
            post.Id = newPost.Id;
            post.SchoolId = newPost.SchoolId;
            post.School = newPost.School;
            post.IsEdited = newPost.IsEdited;

            await repo.SaveChangesAsync();

        }

        public async Task<PostViewModel> GetPostByIdAsync(Guid id)
        {
            var post =await repo.All<Post>().Where(p=>p.Id == id).Include(p=>p.Creator).Include(p=>p.Comments).ThenInclude(p=>p.Creator).FirstOrDefaultAsync();
            if(post is null)
            {
                throw new ArgumentException("Post does not exist.");
            }

            var result = new PostViewModel()
            {
                Id = post.Id,
                Content = post.Content,
                CreatedOn = post.CreatedOn,
                CreatorId = post.CreatorId,
                Creator = new UserViewModel
                {
                    Id = post.Creator.Id,
                    ImageUrl = post.Creator.ImageUrl,
                    Username = post.Creator.UserName
                },
                Comments = post.Comments.Select(c => new CommentViewModel
                {
                    Content = c.Content,
                    CreatedOn = c.CreatedOn,
                    Creator = new UserViewModel
                    {
                        Id = c.Creator.Id,
                        ImageUrl = c.Creator.ImageUrl,
                        Username = c.Creator.UserName
                    }
                }),
                Dislikes = post.Dislikes.Select(d => new PostDislikesViewModel
                {
                    DislikerId = d.UserId,
                    PostId = d.PostId
                }),
                Likes = post.Likes.Select(l=> new PostLikesViewModel
                {
                    LikerId = l.UserId,
                    PostId = l.PostId,
                }),
                LikesCount = post.Likes.Count(),
                DislikesCount = post.Dislikes.Count(),
                SchoolId = post.SchoolId,
            };
            return result;
        }
    }
}
