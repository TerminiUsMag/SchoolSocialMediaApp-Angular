using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using validation = SchoolSocialMediaApp.Common.ValidationConstants;

namespace SchoolSocialMediaApp.Infrastructure.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Comment("Is principal of a school")]
        public bool IsPrincipal { get; set; }

        [Comment("Is teacher in a school")]
        public bool IsTeacher { get; set; }

        [Comment("Is parent in a school")]
        public bool IsParent { get; set; }

        [Comment("Is student in a school")]
        public bool IsStudent { get; set; }

        [Comment("Has a pending invitation for a school")]
        public bool IsInvited { get; set; }

        [Comment("Is admin of the app")]
        public bool IsAdmin { get; set; }

        [Comment("The first name of the user.")]
        [MaxLength(validation.MaxFirstNameLength)]
        [Required]
        public string FirstName { get; set; } = null!;

        [Comment("The last name of the user.")]
        [MaxLength(validation.MaxLastNameLength)]
        [Required]
        public string LastName { get; set; } = null!;

        [Comment("The image url of the user.")]
        [MaxLength(validation.MaxImageUrlLength)]
        [Required]
        public string ImageUrl { get; set; } = null!;

        [Comment("The id of the school which the user is Principal of")]
        public Guid? PrincipledSchoolId { get; set; }

        [Comment("The school which the user is Principal of")]
        [ForeignKey(nameof(PrincipledSchoolId))]
        public School? PrincipledSchool { get; set; }

        [Comment("The id of the school the user is participant in.")]
        public Guid? SchoolId { get; set; }     //Associated school id. Example: If the user is a teacher, then the school id is the id of the school the teacher is teaching in.

        [Comment("The school the user is participant in.")]
        [ForeignKey(nameof(SchoolId))]
        public School? School { get; set; }    //Associated school. Example: If the user is a parent, then the school property is navigation to the school the parent's kid is in.

        [Comment("The parent of the user.")]
        [ForeignKey(nameof(ParentId))]
        public ApplicationUser? Parent { get; set; }     //If the user is a student, then the parent property is navigation to the parent of the student.

        [Comment("The id of the parent of the user.")]
        public Guid? ParentId { get; set; }           //If the user is a student, then the parent id is the id of the parent of the student.


        [Comment("Students of the parent.")]
        [InverseProperty(nameof(ApplicationUser.Parent))]
        public ICollection<ApplicationUser> Students { get; set; } = new List<ApplicationUser>(); //If the user is a parent, then the kids property is navigation to the kids of the parent.

        [Comment("Class of the student")]
        [ForeignKey(nameof(ClassId))]
        public SchoolClass? Class { get; set; } //If the user is a student, the class property is navigation to the class of the student.

        [Comment("The id of the class")]
        public Guid? ClassId { get; set; } //If the user is a student, the class id is the id of the class in which the student is.

        [Comment("Subjects of the teacher")]
        public ICollection<SchoolSubject> Subjects { get; set; } = new List<SchoolSubject>();

        [Comment("The date and time the user was created.")]
        [Required]
        public DateTime CreatedOn { get; set; }

        [Comment("The posts the user has liked.")]
        public ICollection<PostsLikes> LikedPosts { get; set; } = new List<PostsLikes>();

        [Comment("The posts made by the user.")]
        public ICollection<Post> Posts { get; set; } = new List<Post>();

        [Comment("The comments made by the user.")]
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        [Comment("The invitations the user has received.")]
        public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();

        [Comment("The invitations the user has sent.")]
        public ICollection<Invitation> SentInvitations { get; set; } = new List<Invitation>();

        [Comment("Grades of the Student")]
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();

        [Comment("Grades Created")]
        public ICollection<Grade> GradesCreated { get; set; } = new List<Grade>();
    }
}
