using Microsoft.AspNetCore.Authentication;
using System;

namespace Shamyr.Opendentity.Service.OpenId.Exceptions
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
