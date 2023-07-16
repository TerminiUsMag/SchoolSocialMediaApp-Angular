﻿using SchoolSocialMediaApp.ViewModels.Models.Post;

namespace SchoolSocialMediaApp.Core.Contracts
{
    public interface IPostService
    {
        Task<bool> CreatePostAsync(PostCreateModel model, Guid userId);
        Task DeletePostAsync(Guid id, Guid userId);
        Task EditPostAsync(PostEditViewModel model, Guid userId);
        Task<IEnumerable<PostViewModel>> GetAllPostsAsync(Guid schoolId);
        Task<PostViewModel> GetPostByIdAsync(Guid id);
    }
}
