using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using Shamyr.Opendentity.Database;
using Shamyr.Opendentity.Database.Entities;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Shamyr.Opendentity.Service.HostedServices
{
    public class DbInitializationHostedService: IHostedService
    {
        private readonly IServiceProvider fServiceProvider;

        public DbInitializationHostedService(IServiceProvider serviceProvider)
        {
            fServiceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = fServiceProvider.CreateScope();

            await MigrateAsync(scope.ServiceProvider, cancellationToken);
            await InitializeAsync(scope.ServiceProvider, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task MigrateAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var context = serviceProvider.GetRequiredService<DatabaseContext>();
            await context.Database.MigrateAsync(cancellationToken);
        }

        private async Task InitializeAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            if (await roleManager.RoleExistsAsync(Constants._AdminRole))
                return;

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var appManager = serviceProvider.GetRequiredService<OpenIddictApplicationManager<Application>>();

            await roleManager.CreateAsync(new ApplicationRole(Constants._AdminRole));

            await appManager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "default",
                Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.RefreshToken,
                    Permissions.GrantTypes.Password
                }
            }, cancellationToken);

            var user = new ApplicationUser { UserName = "admin" };
            await userManager.CreateAsync(user, "Pass@word1");
        }
    }
}
