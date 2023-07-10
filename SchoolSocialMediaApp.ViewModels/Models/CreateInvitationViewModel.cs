using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;
namespace SchoolSocialMediaApp.ViewModels.Models
{
    public class CreateInvitationViewModel
    {
        [Comment("SenderId is the foreign key of the sender of the invitation.")]
        [Required]
        public Guid SenderId { get; set; }

        [Comment("ReceiverId is the foreign key of the receiver of the invitation.")]
        [Required]
        public Guid ReceiverId { get; set; }

        [Comment("SchoolId is the foreign key of the school the invitation is for.")]
        [Required]
        public Guid SchoolId { get; set; }

        [Comment("Role is the role the invitation is for.")]
        [Required]
        [StringLength(validation.MaxRoleLength, MinimumLength = validation.MinRoleLength)]
        public string Role { get; set; } = null!;

        [Comment("IsAccepted is a boolean that determines if the invitation has been accepted.")]
        public bool? IsAccepted { get; set; }

        [Comment("IsPending is a boolean that determines if the invitation is pending.")]
        public bool? IsPending { get; set; }

        [Comment("IsDeclined is a boolean that determines if the invitation has been declined.")]
        public bool? IsDeclined { get; set; }

        [Comment("CreatedOn is the date and time the invitation was created.")]
        public DateTime? CreatedOn { get; set; }

        [Comment("AcceptedOn is the date and time the invitation was accepted.")]
        public DateTime? AcceptedOn { get; set; }

        [Comment("DeclinedOn is the date and time the invitation was declined.")]
        public DateTime? DeclinedOn { get; set; }

        [Comment("Candidates is a list of users who can be invited to a school.")]
        public List<ApplicationUser> Candidates { get; set; } = new List<ApplicationUser>();
    }
}
