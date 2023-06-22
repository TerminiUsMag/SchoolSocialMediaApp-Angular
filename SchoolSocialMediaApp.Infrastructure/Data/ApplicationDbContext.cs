using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Infrastructure.Data.Models;

namespace SchoolSocialMediaApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>()
                .HasMany(user => user.Posts)
                .WithOne(post => post.Owner)
                .HasForeignKey(post => post.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>()
                .HasMany(user => user.Comments)
                .WithOne(comment => comment.Creator)
                .HasForeignKey(comment => comment.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<ApplicationUser>()
            //    .HasMany(user => user.LikedComments)
            //    .WithMany(comment => comment.Likes);

            //builder.Entity<ApplicationUser>()
            //    .HasMany(user => user.DislikedComments)
            //    .WithMany(comment => comment.Dislikes);

            //builder.Entity<ApplicationUser>()
            //    .HasMany(user => user.LikedPosts)
            //    .WithMany(post => post.Likes);

            //builder.Entity<ApplicationUser>()
            //    .HasMany(user => user.DislikedPosts)
            //    .WithMany(post => post.Dislikes);

            //builder.Entity<Post>()
            //    .HasMany(post => post.Comments)
            //    .WithOne(comment => comment.Post)
            //    .HasForeignKey(comment => comment.PostId)
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Post>()
                .HasMany(post => post.Likes)
                .WithMany(user => user.LikedPosts);

            builder.Entity<Post>()
                .HasMany(post => post.Dislikes)
                .WithMany(user => user.DislikedPosts);

            base.OnModelCreating(builder);
        }
    }
}