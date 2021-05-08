using System;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using Shamyr.Opendentity.Database;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.Service.OpenId;
using Shamyr.Opendentity.Service.OpenId.Factories;
using Shamyr.Opendentity.Service.OpenId.GrantValidators;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
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

            services.Scan(s =>
            {
                s.FromAssemblyOf<OpenIdBuilder>()
                  .AddClasses(c => c.AssignableTo(typeof(IGrantHandler)))
                  .AsImplementedInterfaces()
                  .WithTransientLifetime();
            });
            services.AddTransient<IGrantHandlerFactory, GrantHandlerFactory>();

            return services;
        }

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;

                options.User.RequireUniqueEmail = true;
            });

            services.AddAuthentication()
              .AddGoogle(x =>
              {
                  x.ClientId = "779901881966-s9dvobvmr7d4for8g7huv874nap25jhb.apps.googleusercontent.com";
                  x.ClientSecret = "kn6rf0i2SHlyzMCXqfiAv71H";
              });

            services.AddIdentityCore<ApplicationUser>(opt =>
            {
                opt.Password.RequiredLength = 6;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            })
              .AddSignInManager<SignInManager<ApplicationUser>>()
              .AddRoles<ApplicationRole>()
              .AddRoleManager<RoleManager<ApplicationRole>>()
              .AddEntityFrameworkStores<DatabaseContext>()
              .AddDefaultTokenProviders();

            return services;
        }
    }
}
