using MediatR;
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
    }
}
