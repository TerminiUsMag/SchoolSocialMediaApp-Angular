﻿namespace SchoolSocialMediaApp.ViewModels.Models
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public Guid CreatorId { get; set; }

        public UserViewModel Creator { get; set; } = null!;

        public Guid PostId { get; set; }

        public PostViewModel Post { get; set; } = null!;

        public IEnumerable<CommentsLikesViewModel> Likes { get; set; } = new List<CommentsLikesViewModel>();

        public IEnumerable<CommentsDislikesViewModel> Dislikes { get; set; } = new List<CommentsDislikesViewModel>();
    }
}