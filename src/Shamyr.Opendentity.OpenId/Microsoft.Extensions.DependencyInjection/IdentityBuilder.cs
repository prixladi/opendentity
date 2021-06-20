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
            where T : IdentitySettings
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            ConfigureDefault<IdentityOptions>();
            ConfigureDefault<T>();

            if (typeof(T) != typeof(IdentitySettings))
            {
                ConfigureDefault<IdentitySettings>();
                Services.Configure<IdentitySettings>(configuration);
            }

            Services.Configure<T>(configuration);
            Services.Configure<IdentityOptions>(configuration);

            return this;
        }

        private void ConfigureDefault<T>()
            where T : IdentityOptions
        {
            Services.Configure<T>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = Claims.Role;
                options.ClaimsIdentity.EmailClaimType = Claims.Email;

                options.User.AllowedUserNameCharacters = Utils._AllowedUsernameCharacters;
            });
        }
    }
}
