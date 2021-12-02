using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Opendentity.Database.Entities;
using OpenIddict.Core;

namespace hamyr.Opendentity.OpenId.HostedServices;

using Manager = OpenIddictTokenManager<Token>;

public class TokenPruningService: BackgroundService
{
    private readonly IServiceProvider serviceProvider;

    public TokenPruningService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var scope = serviceProvider.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<TokenPruningService>>();
            var tokenManger = scope.ServiceProvider.GetRequiredService<Manager>();
            try
            {
                var treshold = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1));
                await tokenManger.PruneAsync(treshold, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while pruning tokens.");
            }
            finally
            {
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
