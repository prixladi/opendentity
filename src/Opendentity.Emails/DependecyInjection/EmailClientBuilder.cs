using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Opendentity.Emails.DependencyInjection;

public class EmailClientBuilder
{
    public IServiceCollection Services { get; }

    public EmailClientBuilder(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public EmailClientBuilder ConfigureSettings<T>(IConfigurationSection configuration)
        where T : EmailClientSettings
    {
        if (configuration is null)
            throw new ArgumentNullException(nameof(configuration));

        if (typeof(T) != typeof(EmailClientSettings))
            Services.Configure<EmailClientSettings>(configuration);

        Services.Configure<T>(configuration);

        return this;
    }
}
