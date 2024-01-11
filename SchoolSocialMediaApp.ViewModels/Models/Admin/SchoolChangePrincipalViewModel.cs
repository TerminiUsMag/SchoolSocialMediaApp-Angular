using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSocialMediaApp.ViewModels.Models.Admin
{
    public class SchoolChangePrincipalViewModel
    {
        [Comment("The ID of the school")]
        [Required]
        public Guid SchoolId { get; set; }

        [Comment("The Name of the school")]
        [Required]
        public string SchoolName { get; set; } = null!;

        [Comment("Current principal of the school")]
        [Required]
        public ApplicationUser CurrentPrincipal { get; set; } = null!;

        [Comment("New principal's ID")]
        [Required]
        public Guid NewPrincipalId { get; set; }

        [Comment("Candidates for Principal")]
        public ICollection<ApplicationUser> Candidates { get; set; } = new List<ApplicationUser>();
    }
}
