using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSocialMediaApp.Infrastructure.Common
{
    public  class ValidationConstants
    {
        //Post Validation Constants
        public const int MaxPostLength = 2000;

        //Comment Validation Constants
        public const int MaxCommentLength = 1000;

        //User Validation Constants
        public const int MaxFirstNameLength = 50;
        public const int MaxLastNameLength = 50;
    }
}
