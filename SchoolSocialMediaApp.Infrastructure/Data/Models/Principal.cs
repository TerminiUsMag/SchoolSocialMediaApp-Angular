using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    [Comment("A director of a school.")]
    public class Principal
    {
        [Comment("The id of the director.")]
        [Key]
        public Guid Id { get; set; }

        [Comment("The id of the school the director is in charge of.")]
        [Required]
        public Guid SchoolId { get; set; }

        [Comment("The school the director is in charge of.")]
        [Required]
        [ForeignKey(nameof(SchoolId))]
        public School School { get; set; } = null!;

        [Comment("The id of the user that is a director.")]
        [Required]
        public Guid UserId { get; set; }

        [Comment("The user that is a director.")]
        [Required]
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;
    }
}
