namespace Shamyr.Opendentity.Service
{
    public static class Constants
    {
        public static class Auth
        {
            public const string _AdminRole = "ADMIN";
        }

        public static class CustomStatusCodes
        {
            public const int _EmailNotVerified = 449;
        }

        public static class SettingSections
        {
            public const string _Database = "Database";
            public const string _DatabaseInit = "DatabaseInit";
            public const string _Redis = "Redis";
            public const string _OpenId = "OpenId";
            public const string _Identity = "Identity";
            public const string _Email = "Email";
            public const string _Validation = "Validation";
            public const string _Ui = "Ui";
            public const string _RateLimits = "RateLimits";
        }
    }
}
