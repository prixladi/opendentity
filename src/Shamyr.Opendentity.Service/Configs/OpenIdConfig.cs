using System;
using Shamyr.Opendentity.OpenId;

namespace Shamyr.Opendentity.Service.Configs
{
    public record OpenIdConfig: IOpenIdConfig
    {
        public bool RequireConfirmedAccount { get; } = EnvVariable.TryGetBool(EnvVariables._RequireVerifiedAccount, false);

        public static void Setup(OpenIdBuilder builder)
        {
            builder.AccessTokenDuration = EnvVariable.TryGetTimeSpan(EnvVariables._AccessTokenDuration, TimeSpan.FromMinutes(15));
            builder.RefreshTokenDuration = EnvVariable.TryGetTimeSpan(EnvVariables._RefreshTokenDuration, TimeSpan.FromDays(7));
        }
    }
}
