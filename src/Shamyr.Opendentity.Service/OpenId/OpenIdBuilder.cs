using System;

namespace Shamyr.Opendentity.Service.OpenId
{
    public class OpenIdBuilder
    {
        public TimeSpan AccessTokenDuration { get; set; } = TimeSpan.FromMinutes(30);
        public TimeSpan RefreshTokenDuration { get; set; } = TimeSpan.FromDays(2);
    }
}
