using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Opendentity.Domain.CQRS;
using Opendentity.OpenId;
using OpenIddict.Server.AspNetCore;

namespace Opendentity.Service.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class AuthorizationController: ControllerBase
{
    private readonly ISender sender;

    public AuthorizationController(ISender sender)
    {
        this.sender = sender;
    }

    [HttpPost(OpenIdConstants._TokenRoute), Produces("application/json")]
    public async Task<IActionResult> ConnectTokenAsync(CancellationToken cancellationToken)
    {
        var request = HttpContext.GetOpenIddictServerRequest();
        if (request == null)
        {
            throw new InvalidOperationException("Unable to retrieve OpenIddictServerRequest from HttpContext.");
        }

        var principal = await sender.Send(new TokenCommand(request), cancellationToken);
        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    /// <summary>
    /// Logs current user out by invalidating his refresh tokens, codes etc...
    /// This doesn't affect access tokens if reference tokens are not used.
    /// </summary>
    [HttpPost(OpenIdConstants._LogoutRoute), Produces("application/json")]
    public async Task<IActionResult> LogoutAsync(CancellationToken cancellationToken)
    {
        await sender.Send(new LogoutCommand(), cancellationToken);
        return NoContent();
    }
}
