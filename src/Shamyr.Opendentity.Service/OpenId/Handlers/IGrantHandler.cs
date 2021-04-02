using OpenIddict.Abstractions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Shamyr.Opendentity.Service.OpenId.GrantValidators
{
    public interface IGrantHandler
    {
        bool CanHandle(OpenIddictRequest request);
        Task<ClaimsPrincipal> HandleAsync(OpenIddictRequest request, CancellationToken cancellationToken);
    }
}
