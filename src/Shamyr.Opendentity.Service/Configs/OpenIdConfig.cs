using Shamyr.Opendentity.Service.OpenId;

namespace Shamyr.Opendentity.Service.Configs
{
    public static class OpenIdConfig
    {
        public static void Setup(OpenIdBuilder builder)
        {
            builder.AccessTokenDuration = EnvVariable.GetTimeSpan(EnvVariables._AccessTokenDuration);
            builder.RefreshTokenDuration = EnvVariable.GetTimeSpan(EnvVariables._RefreshTokenDuration);
        }
    }
}
