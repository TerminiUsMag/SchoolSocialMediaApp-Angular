using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using validation = SchoolSocialMediaApp.Common.InfrastructureCommon.ValidationConstantsInfrastructure;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    [Comment("Invitation table holds all the invitations sent to users to join a school and a role in that school. It holds the sender, receiver, school, role, and status of the invitation.")]
    public class Invitation
    {
        [Comment("Id is the primary key of the invitation table.")]
        [Key]
        public Guid Id { get; set; }

        [Comment("SenderId is the foreign key of the sender of the invitation.")]
        [Required]
        public Guid SenderId { get; set; }

        [Comment("Sender is the user who sent the invitation.")]
        [Required]
        [ForeignKey(nameof(SenderId))]
        public ApplicationUser Sender { get; set; } = null!;

        [Comment("ReceiverId is the foreign key of the receiver of the invitation.")]
        [Required]
        public Guid ReceiverId { get; set; }

        [Comment("Receiver is the user who received the invitation.")]
        [Required]
        [ForeignKey(nameof(ReceiverId))]
        public ApplicationUser Receiver { get; set; } = null!;

        [Comment("SchoolId is the foreign key of the school the invitation is for.")]
        [Required]
        public Guid SchoolId { get; set; }

        [Comment("School is the school the invitation is for.")]
        [Required]
        [ForeignKey(nameof(SchoolId))]
        public School School { get; set; } = null!;

        [Comment("Role is the role the invitation is for.")]
        [Required]
        [MaxLength(validation.maxRoleLength)]
        public string Role { get; set; } = null!;

        [Comment("IsAccepted is a boolean that determines if the invitation has been accepted.")]
        public bool IsAccepted { get; set; }

        [Comment("IsPending is a boolean that determines if the invitation is pending.")]
        public bool IsPending { get; set; }

        [Comment("IsDeclined is a boolean that determines if the invitation has been declined.")]
        public bool IsDeclined { get; set; }

        [Comment("CreatedOn is the date and time the invitation was created.")]
        [Required]
        public DateTime CreatedOn { get; set; }

        [Comment("AcceptedOn is the date and time the invitation was accepted.")]
        public DateTime? AcceptedOn { get; set; }

        [Comment("DeclinedOn is the date and time the invitation was declined.")]
        public DateTime? DeclinedOn { get; set; }
    }
}
