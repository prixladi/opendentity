using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;

namespace Shamyr.Opendentity.Service.Configs
{
    public static class DistributedCacheConfig
    {
        public static void SetupRedis(RedisCacheOptions options)
        {
            options.ConfigurationOptions = ConfigurationOptions.Parse(EnvVariable.Get(EnvVariables._RedisConnectionString));
            options.InstanceName = "ac-";
        }
    }
}
