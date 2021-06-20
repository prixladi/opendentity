using System;
using Microsoft.Extensions.Configuration;
using Shamyr.Opendentity.Service.DatabaseInit;

namespace Microsoft.Extensions.DependencyInjection
{
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

        public DatabaseInitBuilder ConfigureSettings(Action<DatabaseInitSettings> configure)
        {
            if (configure is null)
                throw new ArgumentNullException(nameof(configure));

            Services.Configure(configure);

            return this;
        }
    }
}
