using System;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Validation.AspNetCore;
using Shamyr.Opendentity.Database;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.OpenId;
using Shamyr.Opendentity.OpenId.Factories;
using Shamyr.Opendentity.OpenId.Handlers;
using Shamyr.Opendentity.OpenId.Services;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenId<TConfig>(this IServiceCollection services, Action<OpenIdBuilder>? setupBuilder = null)
            where TConfig : class, IOpenIdConfig
        {
            var builder = new OpenIdBuilder();
            setupBuilder?.Invoke(builder);

            services.AddTransient<IOpenIdConfig, TConfig>();
            services.AddTransient<IUserValidationService, UserValidationService>();
            services.AddTransient<ISubjectTokenRevokationService, SubjectTokenRevokationService>();

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

                  opt.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Phone, Scopes.Roles, Scopes.OpenId, Scopes.OfflineAccess);

                  opt.AllowPasswordFlow()
                    .AllowRefreshTokenFlow()
                    .AllowCustomFlow(CustomGrants._Google);

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
                options.ClaimsIdentity.UserNameClaimType = Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = Claims.Role;
                options.ClaimsIdentity.EmailClaimType = Claims.Email;

                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = Utils._AllowedUsernameCharacters;
            });

            services.AddAuthentication(b =>
            {
                b.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
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
              .AddClaimsPrincipalFactory<ClaimsPrincipalFactory>()
              .AddEntityFrameworkStores<DatabaseContext>()
              .AddDefaultTokenProviders();

            return services;
        }
    }
}
