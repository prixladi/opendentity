using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;
using Shamyr.Opendentity.OpenId;
using Shamyr.Opendentity.Service.CQRS.Commands;

namespace Shamyr.Opendentity.Service.Controllers
{
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
    }
}