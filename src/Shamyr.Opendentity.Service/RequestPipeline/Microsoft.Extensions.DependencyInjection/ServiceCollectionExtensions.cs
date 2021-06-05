using MediatR;
using Shamyr.Extensions.DependencyInjection;
using Shamyr.Opendentity.Service;
using Shamyr.Opendentity.Service.RequestPipeline;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediatorPipeline(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            return services;
        }

        public static IServiceCollection AddServiceAssembly(this IServiceCollection services)
        {
            services.Scan(e =>
            {
                e.FromAssemblyOf<Startup>()
                 .AddConventionClasses();
            });

            return services;
        }
    }
}
