using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OpenIddict.Core;
using Shamyr.Linq.Async;
using Shamyr.Opendentity.Database.Entities;

namespace Shamyr.Opendentity.OpenId.Services
{
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
                .ListAsync(q => q.Where(e => e.Subject == subId && e.Status != "revoked"), cancellationToken)
                .ToListAsync(cancellationToken);

            foreach (var token in data)
                await tokenManager.TryRevokeAsync(token, cancellationToken);
        }
    }
}
