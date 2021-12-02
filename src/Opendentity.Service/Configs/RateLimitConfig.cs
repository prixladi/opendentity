using System.Text.Json;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Opendentity.Domain.Models;
using Shamyr.AspNetCore;

namespace Opendentity.Service.Configs;

public static class RateLimitConfig
{
    public static void Setup(IServiceCollection services, IConfigurationSection configuration)
    {
        services.AddMemoryCache();
        services.AddInMemoryRateLimiting();

        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        services.Configure<IpRateLimitOptions>(opt =>
        {
            opt.QuotaExceededResponse = new QuotaExceededResponse
            {
                StatusCode = StatusCodes.Status429TooManyRequests,
                ContentType = "application/json",
                Content = "{" + JsonSerializer.Serialize(new RateLimitExceededModel
                {
                    Message = "Slow down. Maximum allowed requests: {0} per {1}. Please try again in {2} second(s)."
                }, Json.DefaultSerializerOptions) + "}"
            };
        });
        services.Configure<IpRateLimitOptions>(configuration);
    }
}
