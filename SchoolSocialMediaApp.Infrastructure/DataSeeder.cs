using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SchoolSocialMediaApp.Infrastructure.Common;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using System.Data;

namespace SchoolSocialMediaApp.Infrastructure
{
    /// <summary>
    /// Database Data seeder class!
    /// </summary>
    public class DataSeeder
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IServiceProvider serviceProvider;
        public DataSeeder(UserManager<ApplicationUser> _userManager, IServiceProvider _serviceProvider)
        {
            this.userManager = _userManager;
            this.serviceProvider = _serviceProvider;
        }

        public async Task SeedData()
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IRepository>();

                if (!await repo.All<ApplicationRole>().AnyAsync())
                {
                    await SeedRoles(repo);
                }

                if (!await repo.All<ApplicationUser>().AnyAsync())
                {
                    await SeedUsers(repo);
                    await SeedSchool(repo);
                    await FixPrincipalRole(repo);
                    await SeedInvitation(repo);
                    await SeedPosts(repo);
                    await SeedComments(repo);
                    await SeedLikes(repo);
                }
            }
        }

        private async Task SeedInvitation(IRepository repo)
        {
            foreach (var invitation in CreateInvitations())
            {
                await repo.AddAsync(invitation);
            }
            await repo.SaveChangesAsync();
        }

        private List<Invitation> CreateInvitations()
        {
            var result = new List<Invitation>();

            result.Add(new Invitation
            {
                Id = Guid.Parse("608B088D-5B8D-4EA2-B272-451E8FAB872A"),
                SenderId = Guid.Parse("A40FC683-6F20-49F8-1E10-08DB7BFE5717"),
                ReceiverId = Guid.Parse("5DBD2E13-B653-41B3-1E13-08DB7BFE5717"),
                SchoolId = Guid.Parse("F45DBD82-704F-4715-BB70-60CD2F73036A"),
                Role = "Student",
                IsPending = true,
                CreatedOn = DateTime.Now,
            });


            return result;
        }

        private async Task SeedLikes(IRepository repo)
        {
            foreach (var like in CreateLikes())
            {
                await repo.AddAsync(like);
            }

            await repo.SaveChangesAsync();
        }

        private List<PostsLikes> CreateLikes()
        {
            var likes = new List<PostsLikes>();

            likes.Add(new PostsLikes
            {
                PostId = Guid.Parse("FD1EC410-8A72-45B2-871A-08DB9743B838"),
                UserId = Guid.Parse("5DBD2E13-B653-41B3-1E13-08DB7BFE5717")
            });

            return likes;
        }

        private async Task SeedComments(IRepository repo)
        {
            foreach (var comment in CreateComments())
            {
                await repo.AddAsync(comment);
            }

            await repo.SaveChangesAsync();
        }

        private List<Comment> CreateComments()
        {
            var comments = new List<Comment>();

            comments.Add(new Comment
            {
                Id = Guid.Parse("CA825E34-7D34-4067-C39D-08DB9750753B"),
                Content = "Thanks for the support, you are great !",
                CreatedOn = DateTime.Now,
                PostId = Guid.Parse("FD1EC410-8A72-45B2-871A-08DB9743B838"),
                CreatorId = Guid.Parse("5DBD2E13-B653-41B3-1E13-08DB7BFE5717"),

            });


            return comments;
        }

        private async Task SeedPosts(IRepository repo)
        {
            foreach (var post in CreatePosts())
            {
                await repo.AddAsync(post);
            }
            await repo.SaveChangesAsync();
        }

        private List<Post> CreatePosts()
        {
            var posts = new List<Post>();
            posts.Add(new Post
            {
                Id = Guid.Parse("FD1EC410-8A72-45B2-871A-08DB9743B838"),
                Content = "Hello, to all new students and welcome to our school ! :) For any questions feel free to come to my office or the teacher's room, we're here for you all.",
                CreatedOn = DateTime.Now,
                CreatorId = Guid.Parse("A40FC683-6F20-49F8-1E10-08DB7BFE5717"),
                SchoolId = Guid.Parse("F45DBD82-704F-4715-BB70-60CD2F73036A"),
                IsEdited = false,
                Comments = new List<Comment>(),
                Likes = new List<PostsLikes>()
            });

            return posts;
        }

        private async Task FixPrincipalRole(IRepository repo)
        {

            //var role = await repo.All<ApplicationRole>().Where(r => r.Name == "User").FirstOrDefaultAsync();
            var principal = await repo.All<ApplicationUser>().Where(u => u.FirstName == "Principal").FirstOrDefaultAsync();

            var userRoles = await repo.All<IdentityUserRole<Guid>>().Where(ur => ur.UserId == principal.Id).ToListAsync();
            foreach (var userRole in userRoles)
            {
                repo.Delete(userRole);
            }

            var role = await repo.All<ApplicationRole>().Where(r => r.Name == "Principal").FirstOrDefaultAsync();

            await repo.AddAsync<IdentityUserRole<Guid>>(new IdentityUserRole<Guid> { RoleId = role.Id, UserId = principal.Id });
            principal.IsPrincipal = true;
            await repo.SaveChangesAsync();
        }

        private async Task SeedSchool(IRepository repo)
        {
            var school = CreateDemoSchool();
            var principal = await repo.All<ApplicationUser>().Where(u => u.FirstName == "Principal").FirstOrDefaultAsync();

            if (principal is not null)
            {
                school.PrincipalId = principal.Id;
                school.Principal = principal;

                principal.School = school;
                principal.SchoolId = school.Id;
                principal.IsPrincipal = true;

                await repo.AddAsync(school);
                await repo.SaveChangesAsync();
            }
        }

        private School CreateDemoSchool()
        {
            return new School
            {
                Id = Guid.Parse("F45DBD82-704F-4715-BB70-60CD2F73036A"),
                Name = "DEMO School",
                Description = "School used for demo purposes only.",
                ImageUrl = "/images/school-images/demoSchoolProfile.jpg",
                Location = "Sofia, Bulgaria",
            };
        }

        private async Task SeedRoles(IRepository repo)
        {
            foreach (var role in CreateRoles())
            {
                //await roleManager.CreateAsync(role);
                await repo.AddAsync(role);
            }
            await repo.SaveChangesAsync();
        }

        private async Task SeedUsers(IRepository repo)
        {
            foreach (var user in CreateUsers())
            {
                await userManager.CreateAsync(user, "None123");
                if (user.FirstName == "Admin")
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                    user.IsAdmin = true;
                }
                else
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
            //await repo.SaveChangesAsync();
        }

        private List<ApplicationUser> CreateUsers()
        {
            var users = new List<ApplicationUser>();

            //var hasher = new PasswordHasher<ApplicationUser>();

            users.Add(new ApplicationUser
            {
                Id = Guid.Parse("5181f14c-a6fc-464c-1e0f-08db7bfe5717"),
                FirstName = "Admin",
                LastName = "Admin",
                CreatedOn = DateTime.Now,
                UserName = "admin.admin",
                NormalizedUserName = "ADMIN.ADMIN",
                Email = "admin@admins.com",
                NormalizedEmail = "ADMIN@ADMINS.COM",
                EmailConfirmed = false,
                SecurityStamp = "BC7FH6NJQYGF3HPFVFUTZA3OOQPGHZHC",
                ConcurrencyStamp = "5c564bf9-a235-4ec4-8dc4-8bcf12ef4966",
                PhoneNumber = "0000000000",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                ImageUrl = "/images/defaultProfile.png"
            });


            users.Add(new ApplicationUser
            {
                Id = Guid.Parse("a40fc683-6f20-49f8-1e10-08db7bfe5717"),
                FirstName = "Principal",
                LastName = "Principal",
                CreatedOn = DateTime.Now,
                UserName = "principal.principal",
                NormalizedUserName = "PRINCIPAL.PRINCIPAL",
                Email = "principal@principals.com",
                NormalizedEmail = "PRINCIPAL@PRINCIPALS.COM",
                EmailConfirmed = false,
                SecurityStamp = "ZPXT765IMVDPWRJUCGNLC2RDDWZRP6VA",
                ConcurrencyStamp = "0f1bb1eb-c9b9-41cf-b37f-af1c03e4f280",
                PhoneNumber = "1111111111",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                ImageUrl = "/images/user-images/principalProfile.jpg"
            });


            users.Add(new ApplicationUser
            {
                Id = Guid.Parse("37453265-50a9-418b-1e11-08db7bfe5717"),
                FirstName = "Teacher",
                LastName = "Teacher",
                CreatedOn = DateTime.Now,
                UserName = "teacher.teacher",
                NormalizedUserName = "TEACHER.TEACHER",
                Email = "teacher@teachers.com",
                NormalizedEmail = "TEACHER@TEACHERS.COM",
                EmailConfirmed = false,
                SecurityStamp = "BXEWJLAKEB6EO23MT2TY57ETV7XTQVZ6",
                ConcurrencyStamp = "4cf98984-e485-438d-831b-687168281aac",
                PhoneNumber = "2222222222",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                ImageUrl = "/images/defaultProfile.png"
            });


            users.Add(new ApplicationUser
            {
                Id = Guid.Parse("45c2c82d-a4ac-4da1-1e12-08db7bfe5717"),
                FirstName = "Parent",
                LastName = "Parent",
                CreatedOn = DateTime.Now,
                UserName = "parent.parent",
                NormalizedUserName = "PARENT.PARENT",
                Email = "parent@parents.com",
                NormalizedEmail = "PARENT@PARENTS.COM",
                EmailConfirmed = false,
                SecurityStamp = "V3A2ZGPBDCFNLVW2EP5WEVPROE4265ZW",
                ConcurrencyStamp = "6a979441-e166-4d7c-bc4c-4b46e5cae55a",
                PhoneNumber = "3333333333",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                ImageUrl = "/images/defaultProfile.png"
            });


            users.Add(new ApplicationUser
            {
                Id = Guid.Parse("5dbd2e13-b653-41b3-1e13-08db7bfe5717"),
                FirstName = "Student",
                LastName = "Student",
                CreatedOn = DateTime.Now,
                UserName = "student.student",
                NormalizedUserName = "STUDENT.STUDENT",
                Email = "student@students.com",
                NormalizedEmail = "STUDENT@STUDENTS.COM",
                EmailConfirmed = false,
                SecurityStamp = "X64IPK5BPQMIAOA7VWMFQ3NRRAKFHGTD",
                ConcurrencyStamp = "23370626-f28b-4650-8d07-30a2d6be75d3",
                PhoneNumber = "4444444444",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                ImageUrl = "/images/user-images/studentProfile.jpg",
            });

            return users;
        }

        private List<ApplicationRole> CreateRoles()
        {
            var roles = new List<ApplicationRole>();

            roles.Add(new ApplicationRole { Name = "Admin", Id = Guid.Parse("0CD05EE4-8EE2-4765-6C24-08DB9336C9CB"), NormalizedName = "ADMIN", ConcurrencyStamp = "36c04742-c8b6-4505-bf76-0940f859dece" });

            roles.Add(new ApplicationRole { Name = "Student", Id = Guid.Parse("B72853D7-F524-4DF3-400B-08DB929317F6"), NormalizedName = "STUDENT", ConcurrencyStamp = "c24d662a-667a-479d-a9de-147f8cbe733a" });

            roles.Add(new ApplicationRole { Name = "Parent", Id = Guid.Parse("FC07E10C-4081-46B5-400A-08DB929317F6"), NormalizedName = "PARENT", ConcurrencyStamp = "49c2ed70-dcdf-4138-a233-ff107ffaa12d" });

            roles.Add(new ApplicationRole { Name = "Teacher", Id = Guid.Parse("18D54600-8626-46C2-4009-08DB929317F6"), NormalizedName = "TEACHER", ConcurrencyStamp = "32b03111-7e36-4ba4-8298-c0030f823aa9" });

            roles.Add(new ApplicationRole { Name = "Principal", Id = Guid.Parse("96E4D188-68B3-41B3-4008-08DB929317F6"), NormalizedName = "PRINCIPAL", ConcurrencyStamp = "e85834d4-67ad-47b1-a82d-0b284ad885c3" });

            roles.Add(new ApplicationRole { Name = "User", Id = Guid.Parse("FC0E05AB-E3B5-40FE-4007-08DB929317F6"), NormalizedName = "USER", ConcurrencyStamp = "392d1e43-3d13-4663-ba4d-219d98ea625e" });

            return roles;
        }
    }
}
