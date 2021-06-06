using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenIddict.Core;
using Shamyr.Opendentity.Database;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.OpenId.Extensions;
using Shamyr.Opendentity.Service.CQRS.Commands;
using Shamyr.Opendentity.Service.DatabaseInit;
using Shamyr.Opendentity.Service.Services;

namespace Shamyr.Opendentity.Service.CQRS.Handlers
{
    public class InitializeDbCommandHandler: IRequestHandler<InitializeDbCommand>
    {
        private readonly DatabaseContext databaseContext;
        private readonly IDatabaseInitConfig databaseInitConfig;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly OpenIddictApplicationManager<Application> applicationManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IEmailTemplateManager emailTemplateManager;
        private readonly ILogger<InitializeDbCommandHandler> logger;

        private DbSet<Settings> Set => databaseContext.Settings;

        public InitializeDbCommandHandler(
            DatabaseContext databaseContext,
            UserManager<ApplicationUser> userManager,
            OpenIddictApplicationManager<Application> applicationManager,
            RoleManager<ApplicationRole> roleManager,
            IEmailTemplateManager emailTemplateManager,
            IDatabaseInitConfig databaseInitConfig,
            ILogger<InitializeDbCommandHandler> logger)
        {
            this.databaseContext = databaseContext;
            this.userManager = userManager;
            this.applicationManager = applicationManager;
            this.roleManager = roleManager;
            this.emailTemplateManager = emailTemplateManager;
            this.databaseInitConfig = databaseInitConfig;
            this.logger = logger;
        }

        public async Task<Unit> Handle(InitializeDbCommand request, CancellationToken cancellationToken)
        {
            var settings = Set.SingleOrDefault(e => e.Key == Settings._InitKey);
            if (settings is not null)
                return Unit.Value;

            Set.Add(new Settings { Key = Settings._InitKey });
            await databaseContext.SaveChangesAsync(cancellationToken);

            await roleManager.CreateAsync(new ApplicationRole(Constants.Auth._AdminRole));
            var data = await GetInitDataAsync(cancellationToken);

            foreach (var user in data.Users)
            {
                var appUser = user.ToApplicationUser();
                var result = await userManager.CreateAsync(appUser, user.Password);
                if (!result.Succeeded)
                    throw new IdentityException(result);

                if (user.IsAdmin)
                {
                    var roleResult = await userManager.AddToRoleAsync(appUser, Constants.Auth._AdminRole);
                    if (!roleResult.Succeeded)
                        throw new IdentityException(roleResult);
                }
            }

            foreach (var application in data.Applications)
                await applicationManager.CreateAsync(application.ToDescriptor(), cancellationToken);

            return await TryInitializeEmailsAsync(data, cancellationToken);
        }

        private async Task<Unit> TryInitializeEmailsAsync(RootInitDto data, CancellationToken cancellationToken)
        {
            if (data.PasswordResetEmail is not null)
                await emailTemplateManager.CreateEmailTemplateAsync(data.PasswordResetEmail.ToTemplate(EmailTemplateType.PasswordResetEmail), cancellationToken);

            if (data.ConfirmationEmail is not null)
                await emailTemplateManager.CreateEmailTemplateAsync(data.ConfirmationEmail.ToTemplate(EmailTemplateType.ConfirmationEmail), cancellationToken);

            return Unit.Value;
        }

        private async Task<RootInitDto> GetInitDataAsync(CancellationToken cancellationToken)
        {
            if (databaseInitConfig.InitFilePath is null)
            {
                logger.LogWarning($"No '{nameof(databaseInitConfig.InitFilePath)}' was specified, consider configuring database before you start using application.");
                return new RootInitDto();
            }

            using var stream = File.OpenRead(databaseInitConfig.InitFilePath);

            return await JsonSerializer.DeserializeAsync<RootInitDto>(stream, cancellationToken: cancellationToken)
                ?? throw new InvalidOperationException($"Unable to retrive db init root from '{databaseInitConfig.InitFilePath}'");
        }
    }
}
