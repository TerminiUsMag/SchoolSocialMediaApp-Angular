using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSocialMediaApp.Common.InfrastructureCommon
{
    public class ValidationConstantsInfrastructure
    {
        //Post Validation Constants
        public const int MaxPostLength = 2000;
        public const int MinPostLength = 2;
        public const string PostContentRequired = "Post content is required.";
        public const string PostIdRequired = "Post id is required.";
        public const string PostDateAndTimeRequired = "Post creation date and time is required.";
        public const string PostCreatorIdRequired = "Post creator id is required.";
        public const string PostSchoolIdRequired = "Post school id is required.";
        public const string PostCreatorRequired = "Post creator is required.";
        public const string PostSchoolRequired = "Post school is required.";


        //Comment Validation Constants
        public const int MaxCommentLength = 1000;

        //User Validation Constants
        public const int MaxFirstNameLength = 50;
        public const int MaxLastNameLength = 50;

        //Common Validation Constants
        public const int MaxImageUrlLength = 200;

        //School Validation Constants
        public const int MaxSchoolNameLength = 100;
        public const int MaxSchoolDescriptionLength = 1000;
        public const int MaxSchoolLocationLength = 150;

        //Principal Validation Constants

        //Teacher Validation Constants
        public const int MaxSubjectLength = 50;

        //Invitation Validation Constants
        public const int maxRoleLength = 50;
    }
}
