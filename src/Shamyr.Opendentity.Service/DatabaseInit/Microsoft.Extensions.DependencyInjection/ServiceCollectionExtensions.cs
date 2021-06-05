using Shamyr.Opendentity.Service.DatabaseInit;
using Shamyr.Opendentity.Service.DatabaseInit.HostedServices;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseInit<TConfig>(this IServiceCollection services)
            where TConfig : class, IDatabaseInitConfig
        {
            services.AddHostedService<DbInitializationHostedService>();
            services.AddTransient<IDatabaseInitConfig, TConfig>();

            return services;
        }
    }
}
