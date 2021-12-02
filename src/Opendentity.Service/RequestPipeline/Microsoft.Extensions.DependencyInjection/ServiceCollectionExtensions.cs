using MediatR;
using Shamyr.Extensions.DependencyInjection;
using Opendentity.Service.RequestPipeline;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatorPipeline(this IServiceCollection services)
    {
        services.AddMediatR(typeof(Program));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

        return services;
    }

    public static IServiceCollection AddServiceAssembly(this IServiceCollection services)
    {
        services.Scan(e =>
        {
            e.FromAssemblyOf<Program>()
             .AddConventionClasses();
        });

        return services;
    }
}
