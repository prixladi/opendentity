using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Opendentity.Database;
using Opendentity.Database.DependencyInjection;
using Opendentity.Domain.Settings;
using Opendentity.Emails;
using Opendentity.Emails.DependecyInjection;
using Opendentity.OpenId;
using Opendentity.OpenId.DependencyInjection;
using Shamyr.Extensions.DependencyInjection;

namespace Opendentity.Domain.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(
        this IServiceCollection services,
        IConfigurationSection databaseSection,
        IConfigurationSection openIdSection,
        IConfigurationSection emailClientSection,
        IConfigurationSection uiSection,
        IConfigurationSection validationSection)
    {
        services.AddDatabase()
            .ConfigureSettings<DatabaseSettings>(databaseSection);

        services.AddOpenId()
            .ConfigureSettings<OpenIdSettings>(openIdSection);

        services.AddEmailClient()
            .ConfigureSettings<EmailClientSettings>(emailClientSection);

        services.Configure<UISettings>(uiSection);
        services.Configure<ValidationSettings>(validationSection);

        services.Scan(e =>
        {
            e.FromAssembliesOf(typeof(DomainConstants))
             .AddConventionClasses();
        });

        return services;
    }
}
