using System;

namespace Shamyr.Opendentity.OpenId
{
    public record OpenIdBuilder
    {
        public TimeSpan AccessTokenDuration { get; set; }
        public TimeSpan RefreshTokenDuration { get; set; }
    }
}
