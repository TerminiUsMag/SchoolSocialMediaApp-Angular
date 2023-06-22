namespace SchoolSocialMediaApp.Common
{
    public class ValidationConstants
    {
        //RegisterViewModel Validation Constants
        public const int MaxFirstNameLength = 50;
        public const int MinFirstNameLength = 2;
        public const int MaxLastNameLength = 50;
        public const int MinLastNameLength = 2;
        public const int MaxEmailLength = 320;
        public const int MinEmailLength = 3;
        public const int MaxPasswordLength = 30;
        public const int MinPasswordLength = 6;
        public const int MaxPhoneNumberLength = 15;
        public const int MinPhoneNumberLength = 7;
        public const string PasswordsDoNotMatch = "Passwords do not match.";
        public const string InvalidEmail = "Invalid email.";
        public const string InvalidPassword = "Invalid password.";
        public const string InvalidFirstName = "Invalid first name.";
        public const string InvalidLastName = "Invalid last name.";
        public const string InvalidPhoneNumber = "Invalid phone number.";



    }
}
