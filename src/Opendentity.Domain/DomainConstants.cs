namespace Opendentity.Domain;

public static class DomainConstants
{
    public static class Auth
    {
        public const string _AdminRole = "ADMIN";
    }

    public static class CustomStatusCodes
    {
        public const int _EmailNotVerified = 449;
    }

    public static class ErrorCodes
    {
        public const string _UsernameExists = "username_exists";
        public const string _EmailExists = "username_exists";
    }
}
