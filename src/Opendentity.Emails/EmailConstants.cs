namespace Opendentity.Emails;

public static class EmailConstants
{
    public static class ReplacementMarks
    {
        public static class PasswordReset
        {
            public const string _PasswordTokenMark = "{{PASSWORD_TOKEN}}";
            public const string _EmailMark = "{{EMAIL}}";
            public const string _PortalUrlMark = "{{PORTAL_URL}}";
        }

        public static class Confirmation
        {
            public const string _VerifyTokenMark = "{{VERIFY_TOKEN}}";
            public const string _EmailMark = "{{EMAIL}}";
            public const string _PortalUrlMark = "{{PORTAL_URL}}";
        }
    }
}
