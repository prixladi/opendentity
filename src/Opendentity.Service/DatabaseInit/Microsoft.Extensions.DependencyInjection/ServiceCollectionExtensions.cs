using Opendentity.Service.DatabaseInit.HostedServices;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static DatabaseInitBuilder AddDatabaseInit(this IServiceCollection services)
    {
        services.AddHostedService<DbInitializationHostedService>();

        return new DatabaseInitBuilder(services);
    }
}
