using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Shamyr.Opendentity.OpenId;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Microsoft.Extensions.DependencyInjection
{
    public class IdentityBuilder
    {
        public IServiceCollection Services { get; }

        public IdentityBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public IdentityBuilder ConfigureSettings<T>(IConfigurationSection configuration)
            where T : IdentityOptions
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            if (typeof(T) != typeof(IdentityOptions))
                Services.Configure<IdentityOptions>(configuration);

            Services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = Claims.Role;
                options.ClaimsIdentity.EmailClaimType = Claims.Email;

                options.User.AllowedUserNameCharacters = Utils._AllowedUsernameCharacters;
            });

            Services.Configure<T>(configuration);
            return this;
        }

        public IdentityBuilder ConfigureSettings(Action<IdentityOptions> configure)
        {
            if (configure is null)
                throw new ArgumentNullException(nameof(configure));

            Services.Configure(configure);

            return this;
        }
    }
}
