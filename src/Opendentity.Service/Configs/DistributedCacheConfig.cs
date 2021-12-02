using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Opendentity.Service.Settings;
using StackExchange.Redis;

namespace Opendentity.Service.Configs;

public static class DistributedCacheConfig
{
    public static void SetupWithRedis(IServiceCollection services, IConfigurationSection section)
    {
        var redisSettings = section.Get<RedisSettings>();
        services.AddStackExchangeRedisCache(options =>
        {
            options.ConfigurationOptions = ConfigurationOptions.Parse(redisSettings.ConnectionString);
            options.InstanceName = redisSettings.CacheInstanceName;
        });
    }
}
