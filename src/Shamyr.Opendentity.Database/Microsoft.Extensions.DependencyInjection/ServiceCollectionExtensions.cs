using Shamyr.Opendentity.Database;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase<TConfig>(this IServiceCollection services)
            where TConfig : class, IDatabaseConfig
        {
            services.AddDbContext<DatabaseContext>();
            services.AddTransient<IDatabaseConfig, TConfig>();

            return services;
        }
    }
}
