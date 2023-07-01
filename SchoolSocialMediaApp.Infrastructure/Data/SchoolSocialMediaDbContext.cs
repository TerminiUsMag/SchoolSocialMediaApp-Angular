using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Infrastructure.Data.Models;

namespace SchoolSocialMediaApp.Data
{
    public class SchoolSocialMediaDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public SchoolSocialMediaDbContext(DbContextOptions<SchoolSocialMediaDbContext> options)
            : base(options)
        {

        }

        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<CommentsLikes> CommentsLikes { get; set; } = null!;
        public DbSet<CommentsDislikes> CommentsDislikes { get; set; } = null!;

        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<PostsLikes> PostsLikes { get; set; } = null!;
        public DbSet<PostsDislikes> PostsDislikes { get; set; } = null!;

        public DbSet<School> Schools { get; set; } = null!;
        public DbSet<Principal> Principals { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Parent> Parents { get; set; } = null!;
        public DbSet<StudentsParents> StudentsParents { get; set; } = null!;
        public DbSet<Administrator> Administrators { get; set; } = null!;


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

            //builder.Entity<PostsComments>()
            //    .HasKey(pc => new { pc.PostId, pc.CommentId });

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

            builder.Entity<Principal>()
                .HasOne(d => d.School)
                .WithOne(s => s.Principal)
                .HasForeignKey<School>(s => s.PrincipalId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<School>()
                .HasOne(s => s.Principal)
                .WithOne(p => p.School)
                .HasForeignKey<Principal>(p => p.SchoolId);

            builder.Entity<Student>()
                .HasOne(d => d.School)
                .WithMany(s => s.Students)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<StudentsParents>()
                .HasKey(sp => new { sp.StudentId, sp.ParentId });

            builder.Entity<StudentsParents>()
                .HasOne(sp => sp.Parent)
                .WithMany(p => p.Students)
                .HasForeignKey(sp => sp.ParentId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(builder);
        }
    }
}