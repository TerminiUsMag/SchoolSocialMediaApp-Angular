using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using validation = SchoolSocialMediaApp.Common.InfrastructureCommon.ValidationConstantsInfrastructure;

namespace SchoolSocialMediaApp.ViewModels.Models
{
    public class PostCreateModel
    {
        [Comment("Post Content")]
        [Required(ErrorMessage = validation.PostContentRequired)]
        [StringLength(validation.MaxPostLength, MinimumLength = validation.MinPostLength, ErrorMessage = "Post content must be between {1} and {2} characters.")]
        public string Content { get; set; } = null!;

        [Comment("Date and Time of Creation")]
        public DateTime? CreatedOn { get; set; }


    }
}
