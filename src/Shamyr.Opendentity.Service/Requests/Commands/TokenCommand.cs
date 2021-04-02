using System.Security.Claims;
using MediatR;
using OpenIddict.Abstractions;

namespace Shamyr.Opendentity.Service.Requests.Commands
{
    public record TokenCommand(OpenIddictRequest OpenIddictRequest): IRequest<ClaimsPrincipal>;
}
