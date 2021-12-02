using System;
using Microsoft.Extensions.Configuration;
using OpenIddict.Server;
using Opendentity.OpenId;
using Microsoft.Extensions.DependencyInjection;

namespace Opendentity.OpenId.DependencyInjection;

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

        Services.Configure<OpenIddictServerOptions>(configuration);
        Services.Configure<T>(configuration);

        return this;
    }
}
