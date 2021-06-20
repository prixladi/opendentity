﻿using hamyr.Opendentity.OpenId.HostedServices;
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
        public static OpenIdBuilder AddOpenId(this IServiceCollection services)
        {
            services.AddTransient<IUserValidationService, UserValidationService>();
            services.AddTransient<ISubjectTokenRevokationService, SubjectTokenRevokationService>();

            services.AddHostedService<TokenPruningService>();

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
                    .SetUserinfoEndpointUris(OpenIdConstants._UserInfoRoute)
                    .SetLogoutEndpointUris(OpenIdConstants._LogoutRoute);

                  opt.UseAspNetCore()
                    .EnableTokenEndpointPassthrough()
                    .EnableLogoutEndpointPassthrough()
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

            return new OpenIdBuilder(services);
        }

        public static IdentityBuilder AddIdentity(this IServiceCollection services)
        {
            services.AddAuthentication(b =>
            {
                b.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            });

            services.AddIdentityCore<ApplicationUser>()
              .AddSignInManager<SignInManager<ApplicationUser>>()
              .AddRoles<ApplicationRole>()
              .AddRoleManager<RoleManager<ApplicationRole>>()
              .AddClaimsPrincipalFactory<ClaimsPrincipalFactory>()
              .AddEntityFrameworkStores<DatabaseContext>()
              .AddDefaultTokenProviders();

            return new IdentityBuilder(services);
        }
    }
}
