using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Opendentity.Domain.RequestPipeline;
using Opendentity.Service.HostedServices;
using Shamyr.Extensions.DependencyInjection;

namespace Opendentity.Service.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static DatabaseInitBuilder AddDatabaseInit(this IServiceCollection services)
    {
        services.AddHostedService<DbInitializationHostedService>();

        return new DatabaseInitBuilder(services);
    }

    public static IServiceCollection AddMediatorPipeline(this IServiceCollection services)
    {
        services.AddMediatR(typeof(Domain.DomainConstants));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

        return services;
    }

    public static IServiceCollection AddServiceAssembly(this IServiceCollection services)
    {
        services.Scan(e =>
        {
            e.FromAssembliesOf(typeof(Domain.DomainConstants))
             .AddConventionClasses();
        });

        return services;
    }
}
