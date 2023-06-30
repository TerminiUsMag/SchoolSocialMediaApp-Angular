namespace SchoolSocialMediaApp.ViewModels.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;
    }
}
