using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using OpenIddict.Abstractions;

namespace Opendentity.OpenId.Handlers;

public interface IGrantHandler
{
    bool CanHandle(OpenIddictRequest request);
    Task<ClaimsPrincipal> HandleAsync(OpenIddictRequest request, CancellationToken cancellationToken);
}
