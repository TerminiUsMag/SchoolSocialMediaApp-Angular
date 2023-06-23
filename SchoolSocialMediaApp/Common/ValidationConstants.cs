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
        public const string InvalidEmail = "Invalid Email (Must be between {2} and {1} characters).";
        public const string InvalidPassword = "Invalid Password (Must be between {2} and {1} characters).";
        public const string InvalidFirstName = "Invalid First Name (Must be between {2} and {1} characters).";
        public const string InvalidLastName = "Invalid Last Name (Must be between {2} and {1} characters).";
        public const string InvalidPhoneNumber = "Invalid Phone Number (Must be between {2} and {1} characters).";

        public const string RequiredEmailRegisterViewModel = "Email is required for account creation.";
        public const string RequiredPasswordRegisterViewModel = "Password is required for account creation.";
        public const string RequiredConfirmPasswordRegisterViewModel = "Password confirmation is required for account creation.";
        public const string RequiredFirstNameRegisterViewModel = "First name is required for account creation.";
        public const string RequiredLastNameRegisterViewModel = "Last name is required for account creation.";
        public const string RequiredPhoneNumberRegisterViewModel = "Phone number is required for account creation.";


        //LoginViewModel Validation Constants
        public const string InvalidLogin = "Invalid login.";
        public const string RequiredEmailLoginViewModel = "Email is required for login.";
        public const string RequiredPasswordLoginViewModel = "Password is required for login.";

    }
}
