using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Opendentity.Domain.DatabaseInit;

namespace Opendentity.Service.DependencyInjection;

public class DatabaseInitBuilder
{
    public IServiceCollection Services { get; }

    public DatabaseInitBuilder(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public DatabaseInitBuilder ConfigureSettings<T>(IConfigurationSection configuration)
        where T : DatabaseInitSettings
    {
        if (configuration is null)
            throw new ArgumentNullException(nameof(configuration));

        if (typeof(T) != typeof(DatabaseInitSettings))
            Services.Configure<DatabaseInitSettings>(configuration);

        Services.Configure<T>(configuration);

        return this;
    }
}
