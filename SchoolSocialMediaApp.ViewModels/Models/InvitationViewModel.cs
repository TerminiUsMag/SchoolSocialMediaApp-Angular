using Microsoft.EntityFrameworkCore;
using SchoolSocialMediaApp.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;

namespace SchoolSocialMediaApp.ViewModels.Models
{
    public class InvitationViewModel
    {
        [Comment("Id is the primary key of the invitation.")]
        [Required]
        public Guid Id { get; set; }

        [Comment("SenderId is the foreign key of the sender of the invitation.")]
        [Required]
        public Guid SenderId { get; set; }

        [Comment("Sender is the user who sent the invitation.")]
        [Required]
        public ApplicationUser Sender { get; set; } = null!;

        [Comment("ReceiverId is the foreign key of the receiver of the invitation.")]
        [Required]
        public Guid ReceiverId { get; set; }

        [Comment("Receiver is the user who received the invitation.")]
        [Required]
        public ApplicationUser Receiver { get; set; } = null!;

        [Comment("SchoolId is the foreign key of the school the invitation is for.")]
        [Required]
        public Guid SchoolId { get; set; }

        [Comment("Role is the role the invitation is for.")]
        [Required]
        [StringLength(validation.MaxRoleLength, MinimumLength = validation.MinRoleLength)]
        public string Role { get; set; } = null!;

        [Comment("IsPending is a boolean that determines if the invitation is pending.")]
        public bool? IsPending { get; set; }

        [Comment("CreatedOn is the date and time the invitation was created.")]
        public DateTime? CreatedOn { get; set; }
    }
}
