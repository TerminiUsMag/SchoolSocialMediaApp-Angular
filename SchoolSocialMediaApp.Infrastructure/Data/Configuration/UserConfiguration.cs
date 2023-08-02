using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSocialMediaApp.Data;
using SchoolSocialMediaApp.Infrastructure.Common;
using SchoolSocialMediaApp.Infrastructure.Data.Models;

namespace SchoolSocialMediaApp.Infrastructure.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        private readonly SchoolSocialMediaDbContext context;

        public UserConfiguration(SchoolSocialMediaDbContext _context)
        {
            this.context = _context;
        }
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var users = CreateUsers();
            builder.HasData(users);
            AssignUserRoles(users);
        }

        private async void AssignUserRoles(ICollection<ApplicationUser> users)
        {
            foreach (var user in users)
            {
                if (user.FirstName != "Admin")
                {
                    ApplicationRole? role = await context.Roles.Where(r => r.Name == "User").FirstOrDefaultAsync();
                    await context.UserRoles.AddAsync(new IdentityUserRole<Guid> { RoleId = role!.Id, UserId = user.Id });
                }
                else
                {
                    ApplicationRole? role = await context.Roles.Where(r => r.Name == "Admin").FirstOrDefaultAsync();
                    await context.UserRoles.AddAsync(new IdentityUserRole<Guid> { RoleId = role!.Id, UserId = user.Id });
                }
            }
            await context.SaveChangesAsync();
        }

        private List<ApplicationUser> CreateUsers()
        {
            var users = new List<ApplicationUser>();

            var hasher = new PasswordHasher<ApplicationUser>();

            var user = new ApplicationUser
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
            };
            user.PasswordHash = hasher.HashPassword(user, "None123");
            users.Add(user);

            user = new ApplicationUser
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
                ImageUrl = "/images/defaultProfile.png"
            };
            user.PasswordHash = hasher.HashPassword(user, "None123");
            users.Add(user);

            user = new ApplicationUser
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
            };
            user.PasswordHash = hasher.HashPassword(user, "None123");
            users.Add(user);

            user = new ApplicationUser
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
            };
            user.PasswordHash = hasher.HashPassword(user, "None123");
            users.Add(user);

            user = new ApplicationUser
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
                ImageUrl = "/images/defaultProfile.png"
            };
            user.PasswordHash = hasher.HashPassword(user, "None123");
            users.Add(user);

            return users;
        }
    }
}
