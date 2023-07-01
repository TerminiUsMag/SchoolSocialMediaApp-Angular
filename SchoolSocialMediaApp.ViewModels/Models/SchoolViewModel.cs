using Microsoft.EntityFrameworkCore;

namespace SchoolSocialMediaApp.ViewModels.Models
{
    public class SchoolViewModel
    {
        //A view model for the school model
        [Comment("The id of the school.")]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Location { get; set; } = null!;
        public Guid PrincipalId { get; set; }
    }
}
