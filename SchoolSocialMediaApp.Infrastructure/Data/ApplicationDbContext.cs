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
            builder.Entity<CommentsDislikes>()
                .HasKey(cd => new { cd.UserId, cd.CommentId });

            builder.Entity<CommentsLikes>()
                .HasKey(cl => new { cl.UserId, cl.CommentId });

            //builder.Entity<UsersComments>()
            //    .HasKey(uc => new { uc.UserId, uc.CommentId });

            //builder.Entity<UsersPosts>()
            //    .HasKey(up => new { up.UserId, up.PostId });

            builder.Entity<PostsComments>()
                .HasKey(pc => new { pc.PostId, pc.CommentId });

            builder.Entity<PostsDislikes>()
                .HasKey(pd => new { pd.PostId, pd.UserId });

            builder.Entity<PostsLikes>()
                .HasKey(pl => new { pl.PostId, pl.UserId });


            builder.Entity<Comment>()
                .HasOne(Comment => Comment.Creator)
                .WithMany(User => User.Comments)
                .HasForeignKey(Comment => Comment.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Comment>()
                .HasOne(Comment => Comment.Post)
                .WithMany(Post => Post.Comments)
                .HasForeignKey(Comment => Comment.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PostsDislikes>()
                .HasOne(pd => pd.Post)
                .WithMany(p => p.Dislikes)
                .HasForeignKey(pd => pd.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PostsLikes>()
                .HasOne(pl => pl.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(pl => pl.PostId)
                .OnDelete(DeleteBehavior.NoAction);



            base.OnModelCreating(builder);
        }
    }
}