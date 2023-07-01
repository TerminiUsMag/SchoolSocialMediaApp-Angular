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

        //Director Validation Constants

        //Teacher Validation Constants
        public const int MaxSubjectLength = 50;
    }
}
