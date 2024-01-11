using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using SchoolSocialMediaApp.ViewModels.Models.Post;
using SchoolSocialMediaApp.ViewModels.Models.School;
using System.ComponentModel.DataAnnotations;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;

namespace SchoolSocialMediaApp.ViewModels.Models.Admin
{
    public class AdminPanelViewModel
    {
        [Required(ErrorMessage = validation.RequiredSelectedRoleAddUserToRole)]
        public string SelectedRoleId { get; set; } = null!;
        public List<SelectListItem> Roles { get; set; } = new List<SelectListItem>();
        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public List<ApplicationUser> Admins { get; set; } = new List<ApplicationUser>();
        public List<SchoolManageViewModel> Schools { get; set; } = new List<SchoolManageViewModel>();

    }
}
