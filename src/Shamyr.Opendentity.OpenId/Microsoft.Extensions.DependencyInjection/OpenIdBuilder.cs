﻿using System;
using Microsoft.Extensions.Configuration;
using Shamyr.Opendentity.OpenId;

namespace Microsoft.Extensions.DependencyInjection
{
    public class OpenIdBuilder
    {
        public IServiceCollection Services { get; }

        public OpenIdBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public OpenIdBuilder ConfigureSettings<T>(IConfigurationSection configuration)
            where T : OpenIdSettings
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            if (typeof(T) != typeof(OpenIdSettings))
                Services.Configure<OpenIdSettings>(configuration);

            Services.Configure<T>(configuration);

            return this;
        }

        public OpenIdBuilder ConfigureSettings(Action<OpenIdSettings> configure)
        {
            if (configure is null)
                throw new ArgumentNullException(nameof(configure));

            Services.Configure(configure);

            return this;
        }
    }
}
