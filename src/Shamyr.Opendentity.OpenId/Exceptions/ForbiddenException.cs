using System;
using Microsoft.AspNetCore.Authentication;

namespace Shamyr.Opendentity.OpenId.Exceptions
{
    public class ForbiddenException: Exception
    {
        public string Scheme { get; }
        public AuthenticationProperties Properties { get; }

        public ForbiddenException(string scheme, AuthenticationProperties properties)
        {
            Scheme = scheme;
            Properties = properties;
        }
    }
}
