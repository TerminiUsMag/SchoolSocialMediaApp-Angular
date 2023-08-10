using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Core.Contracts;
using SchoolSocialMediaApp.Infrastructure.Common;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.Infrastructure.Migrations;
using SchoolSocialMediaApp.ViewModels.Models.Comment;
using SchoolSocialMediaApp.ViewModels.Models.Post;
using SchoolSocialMediaApp.ViewModels.Models.User;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;

namespace SchoolSocialMediaApp.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository repo;
        private readonly ISchoolService schoolService;
        private readonly IRoleService roleService;

        public PostService(IRepository _repo, ISchoolService _schoolService, IRoleService _roleService)
        {
            this.repo = _repo;
            this.schoolService = _schoolService;
            this.roleService = _roleService;
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

        /// <summary>
        /// Checks if the post is valid.
        /// </summary>
        /// <param name="post"></param>
        /// <exception cref="ArgumentException"></exception>
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


        public async Task<ICollection<PostViewModel>> GetAllPostsAsync(Guid schoolId, Guid userId)
        {
            var posts = await repo.All<Post>().Where(p => p.SchoolId == schoolId).Include(p => p.Likes).OrderByDescending(p => p.CreatedOn).Select(p => new PostViewModel
            {
                Comments = p.Comments.Select(c => new CommentViewModel
                {
                    Id = c.Id,
                    Content = c.Content,
                    Creator = new UserViewModel
                    {
                        Id = c.Creator.Id,
                        ImageUrl = c.Creator.ImageUrl,
                        Username = c.Creator.UserName
                    },
                    CreatedOn = c.CreatedOn,
                    PostId = c.PostId,
                }).ToList(),
                Creator = new UserViewModel
                {
                    Id = p.Creator.Id,
                    ImageUrl = p.Creator.ImageUrl,
                    Username = p.Creator.UserName
                },
                CreatedOn = p.CreatedOn,
                Id = p.Id,
                Likes = p.Likes.Select(l => new PostLikesViewModel
                {
                    LikerId = l.UserId,
                    PostId = l.PostId,
                }).ToList(),
                Content = p.Content,
                CreatorId = p.CreatorId,
                IsEdited = p.IsEdited,
                IsLikedByCurrentUser = p.Likes.Any(l => l.UserId == userId),
            }).ToListAsync();

            return posts;
        }


        public async Task<Guid> EditPostAsync(PostEditViewModel model, Guid userId)
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
                    PostId = c.PostId,
                }).ToList(),
                Content = model.Content,
                CreatedOn = post.CreatedOn,
                //CreatorId = post.CreatorId,
                IsEdited = true,
                SchoolId = post.SchoolId,
                Likes = model.Likes.Select(l => new PostsLikes
                {
                    PostId = l.PostId,
                    UserId = l.LikerId
                }).ToList(),
                Id = model.Id
            };

            post.Comments = newPost.Comments;
            post.Content = newPost.Content;
            post.Likes = newPost.Likes;
            //post.Creator = newPost.Creator;
            //post.CreatorId = newPost.CreatorId;
            post.CreatedOn = newPost.CreatedOn;
            post.Id = newPost.Id;
            post.SchoolId = newPost.SchoolId;
            post.School = newPost.School;
            post.IsEdited = newPost.IsEdited;

            await repo.SaveChangesAsync();
            return post.SchoolId;
        }


        public async Task<PostViewModel> GetPostByIdAsync(Guid id)
        {
            var post = await repo.All<Post>().Where(p => p.Id == id).Include(p => p.Creator).Include(p => p.Comments).ThenInclude(p => p.Creator).FirstOrDefaultAsync();
            if (post is null)
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
                }).ToList(),
                Likes = post.Likes.Select(l => new PostLikesViewModel
                {
                    LikerId = l.UserId,
                    PostId = l.PostId,
                }).ToList(),
                LikesCount = post.Likes.Count(),
                SchoolId = post.SchoolId,
            };
            return result;
        }


        public async Task<Guid> DeletePostAsync(Guid id, Guid userId)
        {
            if (id == Guid.Empty || userId == Guid.Empty)
            {
                throw new ArgumentException("Id is empty.");
            }

            var post = await repo.All<Post>().Include(p => p.Comments).Include(p=>p.School).Include(p=>p.Likes).FirstOrDefaultAsync(p => p.Id == id);
            if (post is null)
            {
                throw new ArgumentException("Post does not exist.");
            }
            var userIsAdmin = await roleService.UserIsInRoleAsync(userId.ToString(), "Admin");
            if (post.CreatorId != userId && post.School.PrincipalId != userId && !userIsAdmin)
            {
                throw new ArgumentException("You are not the creator of this post.");
            }

            var user = await repo.All<ApplicationUser>().FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
            {
                throw new ArgumentException("User does not exist.");
            }

            if (post.Comments.Any())
            {
                var comments = post.Comments.ToList();
                repo.DeleteRange(comments);
            }

            if (post.Likes.Any())
            {
                var likes = post.Likes.ToList();
                repo.DeleteRange(likes);
            }

            repo.Delete(post);
            await repo.SaveChangesAsync();
            return post.SchoolId;
        }


        public async Task LikePostAsync(Guid postId, Guid userId)
        {
            var post = await repo.All<Post>().FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null)
            {
                throw new ArgumentException("Post does not exist.");
            }
            var user = await repo.All<ApplicationUser>().FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            {
                throw new ArgumentException("User does not exist.");
            }
            post.Likes.Add(new PostsLikes
            {
                PostId = postId,
                UserId = userId
            });

            await repo.SaveChangesAsync();
        }


        public async Task UnlikePostAsync(Guid postId, Guid userId)
        {
            var post = await repo.All<Post>().Include(p => p.Likes).FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null)
            {
                throw new ArgumentException("Post does not exist.");
            }
            var user = await repo.All<ApplicationUser>().FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            {
                throw new ArgumentException("User does not exist.");
            }
            var like = post.Likes.FirstOrDefault(l => l.PostId == postId && l.UserId == userId);
            if (like is null)
            {
                throw new ArgumentException("You have not liked this post.");
            }
            post.Likes.Remove(like);

            await repo.SaveChangesAsync();
        }


        public async Task AddCommentAsync(Guid postId, Guid userId, string commentText)
        {
            var post = await repo.All<Post>().FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null)
            {
                throw new ArgumentException("Post does not exist.");
            }
            var user = await repo.All<ApplicationUser>().FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            {
                throw new ArgumentException("User does not exist.");
            }
            var comment = new Comment
            {
                Content = commentText,
                CreatorId = userId,
                PostId = postId,
                CreatedOn = DateTime.Now,
                Creator = user,
                Post = post
            };
            post.Comments.Add(comment);
            await repo.SaveChangesAsync();
        }


        public async Task<List<CommentViewModel>> GetCommentsByPostIdAsync(Guid postId)
        {
            var post = await repo.All<Post>().Include(p => p.Comments).ThenInclude(c => c.Creator).FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null)
            {
                throw new ArgumentException("Post does not exist.");
            }
            var comments = post.Comments.OrderByDescending(c => c.CreatedOn).Select(c => new CommentViewModel
            {
                Content = c.Content,
                CreatedOn = c.CreatedOn,
                Creator = new UserViewModel
                {
                    Id = c.Creator.Id,
                    ImageUrl = c.Creator.ImageUrl,
                    Username = c.Creator.UserName
                },
                CreatorId = c.CreatorId,
                PostId = c.PostId,
                Id = c.Id
            }).ToList();

            return comments;
        }


        public async Task DeleteCommentAsync(Guid commentId)
        {
            var comment = await repo.All<Comment>().Include(c => c.Post).Where(c => c.Id == commentId).FirstOrDefaultAsync();
            if (comment is null)
            {
                throw new ArgumentException("The comment does not exist.");
            }

            repo.Delete(comment);
            await repo.SaveChangesAsync();
        }
    }
}
