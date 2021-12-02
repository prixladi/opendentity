using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using Opendentity.Database;
using Polly;
using Opendentity.Service.CQRS.Commands;

namespace Opendentity.Service.DatabaseInit.HostedServices;

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
        var logger = serviceProvider.GetRequiredService<ILogger<DbInitializationHostedService>>();

        var pol = Policy
            .Handle<SocketException>()
            .OrInner<SocketException>()
            .Or<PostgresException>(ex => ex.SqlState == PostgresErrorCodes.CannotConnectNow)
            .WaitAndRetryAsync(
                int.MaxValue,
                retryAttemp => TimeSpan.FromSeconds(Math.Min(Math.Pow(2, retryAttemp), 60)),
                (ex, time, context) => logger.LogError(ex, "Exception while migrating db, elapsed:{totalSeconds}s", time.TotalSeconds.ToString("n1")));

        await pol.ExecuteAsync(async () => await context.Database.MigrateAsync(cancellationToken));
    }

    private async Task InitializeAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var sender = serviceProvider.GetRequiredService<ISender>();
        await sender.Send(new InitializeDbCommand(), cancellationToken);
    }
}
