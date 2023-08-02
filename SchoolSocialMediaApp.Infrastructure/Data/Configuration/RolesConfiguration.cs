using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSocialMediaApp.Infrastructure.Data.Models;

namespace SchoolSocialMediaApp.Infrastructure.Data.Configuration
{
    public class RolesConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData(CreateRoles());
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
