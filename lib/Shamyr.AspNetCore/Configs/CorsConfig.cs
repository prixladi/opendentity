using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Shamyr.AspNetCore.Configs
{
    public static class CorsConfig
    {
        public const string _AllowAnyCorsPolicy = "AllowAny";

        public static void SetupAllowAny(CorsOptions options)
        {
            options.AddPolicy(_AllowAnyCorsPolicy,
            builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        }
    }
}
