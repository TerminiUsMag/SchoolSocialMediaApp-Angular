using Microsoft.AspNetCore.Identity;
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
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Comment("The first name of the user.")]
        [Required]
        public string FirstName { get; set; } = null!;

        [Comment("The last name of the user.")]
        [Required]
        public string LastName { get; set; } = null!;

        [Comment("The date and time the user was created.")]
        [Required]
        public DateTime CreatedOn{ get; set; }

        [Comment("The posts the user has liked.")]
        public IEnumerable<Post> LikedPosts { get; set; } = new List<Post>();

        [Comment("The posts the user has disliked.")]
        public IEnumerable<Post> DislikedPosts { get; set; } = new List<Post>();

        [Comment("The comments the user has liked.")]
        public IEnumerable<Comment> LikedComments { get; set; } = new List<Comment>();

        [Comment("The comments the user has disliked.")]
        public IEnumerable<Comment> DislikedComments { get; set; } = new List<Comment>();

        [Comment("The posts made by the user.")]
        public IEnumerable<Post> Posts { get; set; } = new List<Post>();

        [Comment("The comments made by the user.")]
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
    }
}
