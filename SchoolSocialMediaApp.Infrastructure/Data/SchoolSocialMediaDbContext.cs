using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using System.Reflection.Emit;

namespace SchoolSocialMediaApp.Data
{
    public class SchoolSocialMediaDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public SchoolSocialMediaDbContext(DbContextOptions<SchoolSocialMediaDbContext> options)
            : base(options)
        {

        }

        public DbSet<Comment> Comments { get; set; } = null!;

        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<PostsLikes> PostsLikes { get; set; } = null!;

        public DbSet<School> Schools { get; set; } = null!;
        public DbSet<Invitation> Invitations { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder builder)
        {

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

            builder.Entity<PostsLikes>()
                .HasOne(pl => pl.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(pl => pl.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ApplicationUser>()
                .HasOne(d => d.School)
                .WithOne(s => s.Principal)
                .HasForeignKey<School>(s => s.PrincipalId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.HasIndex(u => u.SchoolId).IsUnique(false);
            });

            builder.Entity<School>()
                .HasOne(s => s.Principal)
                .WithOne(p => p.School)
                .HasForeignKey<ApplicationUser>(p => p.SchoolId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Invitation>()
                .HasOne(i => i.School)
                .WithMany(s => s.Invitations)
                .HasForeignKey(i => i.SchoolId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Invitation>()
                .HasOne(i=>i.Receiver)
                .WithMany(r=>r.Invitations)
                .HasForeignKey(i=>i.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Invitation>()
                .HasOne(i => i.Sender)
                .WithMany(s => s.SentInvitations)
                .HasForeignKey(i => i.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(builder);
        }
    }
}