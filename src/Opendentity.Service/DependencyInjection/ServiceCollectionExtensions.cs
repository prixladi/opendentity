using System.Text.Json;
using AspNetCoreRateLimit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Opendentity.Domain;
using Opendentity.Domain.DatabaseInit;
using Opendentity.Domain.Models;
using Opendentity.Domain.RequestPipeline;
using Opendentity.OpenId;
using Opendentity.OpenId.DependencyInjection;
using Opendentity.Service.HostedServices;
using Shamyr.AspNetCore;
using Shamyr.Extensions.DependencyInjection;

namespace Opendentity.Service.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddService(
        this IServiceCollection services,
        IConfigurationSection databaseInitSection,
        IConfigurationSection identitySection,
        IConfigurationSection rateLimitSection)
    {
        services.AddDatabaseInit()
            .ConfigureSettings<DatabaseInitSettings>(databaseInitSection);

        services.AddIdentityAuth()
            .ConfigureSettings<IdentitySettings>(identitySection);

        services.AddRateLimiting(rateLimitSection);

        services.AddMediatorPipeline();

        services.Scan(e =>
        {
            e.FromAssembliesOf(typeof(Constants))
             .AddConventionClasses();
        });

        return services;
    }

    private static DatabaseInitBuilder AddDatabaseInit(this IServiceCollection services)
    {
        services.AddHostedService<DbInitializationHostedService>();

        return new DatabaseInitBuilder(services);
    }

    private static IServiceCollection AddMediatorPipeline(this IServiceCollection services)
    {
        services.AddMediatR(typeof(DomainConstants));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

        return services;
    }

    private static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfigurationSection configuration)
    {
        services.AddMemoryCache();
        services.AddInMemoryRateLimiting();

        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        services.Configure<IpRateLimitOptions>(opt =>
        {
            opt.QuotaExceededResponse = new QuotaExceededResponse
            {
                StatusCode = StatusCodes.Status429TooManyRequests,
                ContentType = "application/json",
                Content ="{" + JsonSerializer.Serialize(new RateLimitExceededModel
                {
                    Message = "Slow down. Maximum allowed requests: {0} per {1}. Please try again in {2} second(s)."
                }, Json.DefaultSerializerOptions) + "}"
            };
        });
        services.Configure<IpRateLimitOptions>(configuration);

        return services;
    }
}
