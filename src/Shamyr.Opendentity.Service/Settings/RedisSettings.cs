namespace Shamyr.Opendentity.Service.Settings
{
    public record RedisSettings
    {
        public string ConnectionString { get; init; } = default!;
        public string CacheInstanceName { get; init; } = default!;
    }
}
