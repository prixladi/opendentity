using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shamyr.Opendentity.Database;
using Shamyr.Opendentity.Service.CQRS.Commands;

namespace Shamyr.Opendentity.Service.HostedServices
{
    public class DbInitializationHostedService: IHostedService
    {
        private readonly IServiceProvider serviceProvider;

        public DbInitializationHostedService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = serviceProvider.CreateScope())
                await MigrateAsync(scope.ServiceProvider, cancellationToken);

            using (var scope = serviceProvider.CreateScope())
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
            var sender = serviceProvider.GetRequiredService<ISender>();
            await sender.Send(new InitializeDbCommand(), cancellationToken);
        }
    }
}
