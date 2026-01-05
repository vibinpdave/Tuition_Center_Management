namespace TCM.Application.Common.Constants
{
    public static class RegexPatterns
    {
        // India mobile number (starts with 6–9, total 10 digits)
        public const string MOBILE_NUMBER = @"^[6-9]\d{9}$";
        public const string PASSWORD =
            @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";
    }
}
