using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Principal;

namespace SchoolSocialMediaApp.Infrastructure.Data
{
    public class DataInitializer
    {
        private readonly ModelBuilder modelBuilder;

        public DataInitializer(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }

        public void Seed()
        {
            modelBuilder.Entity<ApplicationUser>().HasData(
                   new ApplicationUser
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
                       PasswordHash = "AQAAAAEAACcQAAAAEHXXpBRNwAwjpW1c1l7UJH017g02XEOy7Mq1UiyZL72s81hxCT/llfGa00s2z4qU4g==",
                       SecurityStamp = "BC7FH6NJQYGF3HPFVFUTZA3OOQPGHZHC",
                       ConcurrencyStamp = "5c564bf9-a235-4ec4-8dc4-8bcf12ef4966",
                       PhoneNumber = "0000000000",
                       PhoneNumberConfirmed = false,
                       TwoFactorEnabled = false,
                       LockoutEnd = null,
                       LockoutEnabled = true,
                       AccessFailedCount = 0,
                       ImageUrl = "/images/defaultProfile.png"
                   },
                   new ApplicationUser
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
                       PasswordHash = "AQAAAAEAACcQAAAAECT1+m0nz+xSKWwOiuBFJWgxAzdl4p0EvA2sI1sAIaXA7MKR78e8adnbdzaB0goFBg==",
                       SecurityStamp = "ZPXT765IMVDPWRJUCGNLC2RDDWZRP6VA",
                       ConcurrencyStamp = "0f1bb1eb-c9b9-41cf-b37f-af1c03e4f280",
                       PhoneNumber = "1111111111",
                       PhoneNumberConfirmed = false,
                       TwoFactorEnabled = false,
                       LockoutEnd = null,
                       LockoutEnabled = true,
                       AccessFailedCount = 0,
                       ImageUrl = "/images/defaultProfile.png"
                   }

            ) ;
        }
    }
}
