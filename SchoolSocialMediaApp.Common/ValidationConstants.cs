namespace SchoolSocialMediaApp.Common
{
    public class ValidationConstants
    {
                                                       //INFRASTRUCTURE Validation Constants

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
        public const int MaxRoleLength = 10;
        public const int MinRoleLength = 2;

        //School Subject Validation Constants
        public const int MaxSchoolSubjectNameLength = 100;
        public const int MinSchoolSubjectNameLength = 2;

        //School Class Validation Constants
        public const int MaxSchoolClassNameLength = 20;
        public const int MinSchoolClassNameLength = 1;
        public const int MaxSchoolClassGrade = 12;
        public const int MinSchoolClassGrade = 1;
        public const string SchoolClassNameRequired = "School class name is required.";
        public const string SchoolClassGradeRequired = "School class grade is required.";


                                                         //CORE Validation Constants

        //Login and Register Validation Constants
        public const string PhoneNumberRegEx = @"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$";

        //WEB Validation Constants

        //User Validation Constants
        public const int MaxFirstNameLength = 50;
        public const int MinFirstNameLength = 2;

        public const int MaxLastNameLength = 50;
        public const int MinLastNameLength = 2;

        public const int MaxUsernameLength = 100;
        public const int MinUsernameLength = 2;

        public const int MaxEmailLength = 320;
        public const int MinEmailLength = 3;

        public const int MaxPasswordLength = 30;
        public const int MinPasswordLength = 6;

        public const int MaxPhoneNumberLength = 15;
        public const int MinPhoneNumberLength = 9;

        public const string InvalidFirstName = "Invalid First Name (Must be between {2} and {1} characters).";
        public const string InvalidLastName = "Invalid Last Name (Must be between {2} and {1} characters).";
        public const string InvalidUsername = "Invalid Username (Must be between {2} and {1} characters).";
        public const string InvalidEmail = "Invalid Email (Must be between {2} and {1} characters).";
        public const string InvalidPhoneNumber = "Invalid Phone Number (Must be between {2} and {1} characters).";
        public const string InvalidPassword = "Invalid Password (Must be between {2} and {1} characters).";
        public const string PasswordsDoNotMatch = "Passwords do not match.";

        public const string RequiredFirstName = "First name is required.";
        public const string RequiredLastName = "Last name is required.";
        public const string RequiredUsername = "Username is required.";
        public const string RequiredEmail = "Email is required.";
        public const string RequiredPhoneNumber = "Phone number is required.";
        public const string RequiredPassword = "Password is required.";
        public const string RequiredConfirmPassword = "Password confirmation is required.";
        public const string RequiredImageUrl = "Image url is required.";
        public const string RequiredId = "Id is required.";

        public const string RequiredEmailRegisterViewModel = "Email is required for account creation.";
        public const string RequiredPasswordRegisterViewModel = "Password is required for account creation.";
        public const string RequiredConfirmPasswordRegisterViewModel = "Password confirmation is required for account creation.";
        public const string RequiredFirstNameRegisterViewModel = "First name is required for account creation.";
        public const string RequiredLastNameRegisterViewModel = "Last name is required for account creation.";
        public const string RequiredPhoneNumberRegisterViewModel = "Phone number is required for account creation.";

        //School Class ViewModel Constants
        public const string InvalidSchoolClassName = "Invalid School Class Name (Must be between {2} and {1} characters).";
        public const string InvalidSchoolClassGrade = "Invalid School Class Grade (Must be a number between {2} and {1}).";

        //School Subject ViewModel Constants
        public const string InvalidSchoolSubjectName = "Invalid School Subject Name (Must be between {2} and {1} characters).";

        //LoginViewModel Validation Constants
        public const string InvalidLogin = "Invalid login.";
        public const string RequiredEmailLoginViewModel = "Email is required for login.";
        public const string RequiredPasswordLoginViewModel = "Password is required for login.";

        //SchoolViewModel Validation Constants
        public const int MinSchoolNameLength = 2;
        public const int MinSchoolDescriptionLength = 5;
        public const int MinImageUrlLength = 8;
        public const int MinSchoolLocationLength = 3;

        //User Deletion
        public const string RequiredPasswordDeleteViewModel = "Password is required for account deletion.";

        //School Deletion
        public const string RequiredPasswordDeleteSchoolViewModel = "Password is required for school deletion.";

        //User Quit School
        public const string RequiredPasswordQuitViewModel = "Password is required to Quit school.";

        //Make User Admin
        public const string RequiredPasswordMakeAdmin = "Admin password confirmation is required to give admin permissions.";
    }
}
