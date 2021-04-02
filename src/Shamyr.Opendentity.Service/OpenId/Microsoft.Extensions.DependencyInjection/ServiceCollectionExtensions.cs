using System;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using Shamyr.Opendentity.Database;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.Service.OpenId;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenId(this IServiceCollection services, Action<OpenIdBuilder>? setupBuilder = null)
        {
            var builder = new OpenIdBuilder();
            setupBuilder?.Invoke(builder);

            services.AddOpenIddict()
              .AddCore(opt =>
              {
                  opt.UseEntityFrameworkCore(builder =>
                  {
                      builder.ReplaceDefaultEntities<Application, Authorization, Scope, Token, string>();
                      builder.UseDbContext<DatabaseContext>();
                  });
              })
              .AddServer(opt =>
              {
                  opt.SetTokenEndpointUris(OpenIdConstants._TokenRoute)
                    .SetUserinfoEndpointUris(OpenIdConstants._UserInfoRoute);

                  opt.SetAccessTokenLifetime(builder.AccessTokenDuration);
                  opt.SetRefreshTokenLifetime(builder.RefreshTokenDuration);

                  opt.UseAspNetCore()
                    .EnableTokenEndpointPassthrough()
                    .DisableTransportSecurityRequirement();

                  opt.AddDevelopmentEncryptionCertificate();
                  opt.AddDevelopmentSigningCertificate();

                  opt.AllowPasswordFlow()
                    .AllowRefreshTokenFlow();

                  opt.DisableAccessTokenEncryption();
              })
              .AddValidation(opt =>
              {
                  opt.UseLocalServer();
                  opt.UseAspNetCore();
              });

            return services;
        }

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            }).AddIdentityCookies();

            services.AddIdentityCore<ApplicationUser>(opt =>
            {
                opt.Password.RequiredLength = 6;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            })
              .AddRoles<ApplicationRole>()
              .AddSignInManager<SignInManager<ApplicationUser>>()
              .AddEntityFrameworkStores<DatabaseContext>()
              .AddDefaultTokenProviders();

            return services;
        }
    }
}
