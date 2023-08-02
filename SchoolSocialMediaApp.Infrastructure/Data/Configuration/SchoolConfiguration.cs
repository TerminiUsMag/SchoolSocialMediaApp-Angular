using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSocialMediaApp.Infrastructure.Data.Models;

namespace SchoolSocialMediaApp.Infrastructure.Data.Configuration
{
    public class SchoolConfiguration : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            builder.HasData(CreateSchools());
        }

        private List<School> CreateSchools()
        {
            var schools = new List<School>();

            schools.Add(new School
            {
                Name = "DEMO School",
                Description = "School used for demo purposes only.",
                ImageUrl = "/images/school-images",
                Location = "Sofia, Bulgaria",

            });

            return schools;
        }
    }
}
