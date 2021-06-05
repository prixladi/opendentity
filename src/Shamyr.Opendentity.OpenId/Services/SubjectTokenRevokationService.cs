using System.Threading;
using System.Threading.Tasks;
using OpenIddict.Core;
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
            await foreach (var token in tokenManager.FindBySubjectAsync(subId, cancellationToken))
                await tokenManager.TryRevokeAsync(token, cancellationToken);
        }
    }
}
