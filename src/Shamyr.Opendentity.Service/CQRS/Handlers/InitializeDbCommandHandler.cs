using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Core;
using Shamyr.Opendentity.Database;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.Service.Configs;
using Shamyr.Opendentity.Service.CQRS.Commands;
using Shamyr.Opendentity.Service.DatabaseInit;

namespace Shamyr.Opendentity.Service.CQRS.Handlers
{
    public class InitializeDbCommandHandler: IRequestHandler<InitializeDbCommand>
    {
        private readonly DatabaseContext databaseContext;
        private readonly IDatabaseInitConfig databaseInitConfig;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly OpenIddictApplicationManager<Application> applicationManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        private DbSet<Settings> Set => databaseContext.Settings;

        public InitializeDbCommandHandler(
            DatabaseContext databaseContext,
            IDatabaseInitConfig databaseInitConfig,
            UserManager<ApplicationUser> userManager,
            OpenIddictApplicationManager<Application> applicationManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this.databaseContext = databaseContext;
            this.databaseInitConfig = databaseInitConfig;
            this.userManager = userManager;
            this.applicationManager = applicationManager;
            this.roleManager = roleManager;
        }

        public async Task<Unit> Handle(InitializeDbCommand request, CancellationToken cancellationToken)
        {
            var settings = Set.SingleOrDefault(e => e.Key == Settings._InitKey);
            if (settings is not null)
                return Unit.Value;

            Set.Add(new Settings { Key = Settings._InitKey });
            await databaseContext.SaveChangesAsync(cancellationToken);

            await roleManager.CreateAsync(new ApplicationRole(Constants._AdminRole));

            var data = await GetInitDataAsync(cancellationToken);

            foreach (var user in data.Users)
            {
                var appUser = user.ToApplicationUser();
                var result = await userManager.CreateAsync(appUser, user.Password);
                if (!result.Succeeded)
                    throw new InvalidOperationException(result.ToString());

                if (user.IsAdmin)
                {
                   var roleResult = await userManager.AddToRoleAsync(appUser, Constants._AdminRole);
                    if (!roleResult.Succeeded)
                        throw new InvalidOperationException(roleResult.ToString());
                }
            }

            foreach (var application in data.Applications)
                await applicationManager.CreateAsync(application.ToDescriptor(), cancellationToken);

            return Unit.Value;
        }

        private async Task<RootInitDto> GetInitDataAsync(CancellationToken cancellationToken)
        {
            if (databaseInitConfig.InitFilePath is null)
                return new RootInitDto();

            using var stream = File.OpenRead(databaseInitConfig.InitFilePath);

            return await JsonSerializer.DeserializeAsync<RootInitDto>(stream, cancellationToken: cancellationToken)
                ?? throw new InvalidOperationException($"Unable to retrive db init root from '{databaseInitConfig.InitFilePath}'");
        }
    }
}
