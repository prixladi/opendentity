using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Opendentity.Database.Entities;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using Shamyr.Linq.Async;

namespace Opendentity.OpenId.Services;

public class SubjectTokenRevokationService: ISubjectTokenRevokationService
{
    private readonly OpenIddictTokenManager<Token> tokenManager;

    public SubjectTokenRevokationService(OpenIddictTokenManager<Token> tokenManager)
    {
        this.tokenManager = tokenManager;
    }

    public async Task RevokeAllAsync(string subId, CancellationToken cancellationToken)
    {
        var data = await tokenManager
            .ListAsync(q => q.Where(
                e => e.Subject == subId &&
                e.Status != OpenIddictConstants.Statuses.Revoked &&
                e.Status != OpenIddictConstants.Statuses.Rejected &&
                e.Status != OpenIddictConstants.Statuses.Inactive), cancellationToken)
            .ToListAsync(cancellationToken);

        foreach (var token in data)
            await tokenManager.TryRevokeAsync(token, cancellationToken);
    }
}
