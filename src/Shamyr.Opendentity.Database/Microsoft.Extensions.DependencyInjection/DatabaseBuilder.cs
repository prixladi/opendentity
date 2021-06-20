using System;
using Microsoft.Extensions.Configuration;
using Shamyr.Opendentity.Database;

namespace Microsoft.Extensions.DependencyInjection
{
    public class DatabaseBuilder
    {
        public IServiceCollection Services { get; }

        public DatabaseBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public DatabaseBuilder ConfigureSettings<T>(IConfigurationSection configuration)
            where T : DatabaseSettings
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            if (typeof(T) != typeof(DatabaseSettings))
                Services.Configure<DatabaseSettings>(configuration);

            Services.Configure<T>(configuration);

            return this;
        }

        public DatabaseBuilder ConfigureSettings(Action<DatabaseSettings> configure)
        {
            if (configure is null)
                throw new ArgumentNullException(nameof(configure));

            Services.Configure(configure);

            return this;
        }
    }
}
