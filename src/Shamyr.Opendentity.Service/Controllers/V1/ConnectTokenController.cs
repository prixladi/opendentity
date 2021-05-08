using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;
using Shamyr.Opendentity.Service.CQRS.Commands;
using Shamyr.Opendentity.Service.OpenId;

namespace Shamyr.Opendentity.Service.Controllers
{
    [ApiController]
    public class ConnectTokenController: ControllerBase
    {
        private readonly ISender sender;

        public ConnectTokenController(ISender sender)
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