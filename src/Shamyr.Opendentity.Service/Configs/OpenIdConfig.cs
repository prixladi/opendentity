using System;
using Shamyr.Opendentity.Service.OpenId;

namespace Shamyr.Opendentity.Service.Configs
{
    public static class OpenIdConfig
    {
        public static void Setup(OpenIdBuilder builder)
        {
            builder.AccessTokenDuration = EnvVariable.TryGetTimeSpan(EnvVariables._AccessTokenDuration, TimeSpan.FromMinutes(15));
            builder.RefreshTokenDuration = EnvVariable.TryGetTimeSpan(EnvVariables._RefreshTokenDuration, TimeSpan.FromDays(7));
        }
    }
}
