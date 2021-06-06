using System;
using Microsoft.AspNetCore.Identity;

namespace Shamyr.Opendentity.OpenId.Extensions
{
    public sealed class IdentityException: Exception
    {
        public IdentityResult Result { get; }

        public IdentityException(IdentityResult result)
        {
            Result = result ?? throw new ArgumentNullException(nameof(result));
        }
    }
}
