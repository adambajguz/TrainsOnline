namespace TrainsOnline.Application.Constants
{
    public static class ValidationMessages
    {
        public static class Auth
        {
            public const string EmailOrPasswordIsIncorrect = "e-mail or password is incorrect";
        }

        public static class Email
        {
            public const string IsEmpty = "e-mail must not be empty";
            public const string HasWrongFormat = "e-mail must have a valid format";

            public const string IsInUse = "e-mail is already in use";
        }

        public static class Password
        {
            public const string IsEmpty = "Password must not be empty";
            public const string IsTooShort = "Password must have at least {0} characters";
            public const string IsTooLong = "Password must have less than {0} characters";

            //public const string NewIsEqualToOld = "New password must not equal to old password";
            public const string OldIsIncorrect = "Old password is incorrect";
        }

        public static class General
        {
            public const string IsIncorrectId = "Id is incorrect";
            public const string IsNullOrEmpty = "Is null or empty";
            public const string GreaterThenZero = "Number should be greater than 0";
        }
    }
}
