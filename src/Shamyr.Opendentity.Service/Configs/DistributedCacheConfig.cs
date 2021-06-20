using Microsoft.Extensions.DependencyInjection;
using Shamyr.Opendentity.Service.Settings;
using StackExchange.Redis;

namespace Shamyr.Opendentity.Service.Configs
{
    public static class DistributedCacheConfig
    {
        public static void SetupWithRedis(IServiceCollection services, RedisSettings redisSettings)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = ConfigurationOptions.Parse(redisSettings.ConnectionString);
                options.InstanceName = redisSettings.CacheInstanceName;
            });
        }
    }
}
