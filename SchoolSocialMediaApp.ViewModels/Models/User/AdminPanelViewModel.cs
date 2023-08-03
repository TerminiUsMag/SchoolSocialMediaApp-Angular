using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.Post;
using SchoolSocialMediaApp.ViewModels.Models.School;

namespace SchoolSocialMediaApp.ViewModels.Models.User
{
    public class AdminPanelViewModel
    {
        public List<PostViewModel> Posts { get; set; } = new List<PostViewModel>();
        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public List<SchoolManageViewModel> Schools { get; set; } = new List<SchoolManageViewModel>();

    }
}
