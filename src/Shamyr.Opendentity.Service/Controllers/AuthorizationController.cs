using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;
using Shamyr.Opendentity.Service.OpenId;
using Shamyr.Opendentity.Service.Requests.Commands;

namespace Shamyr.Opendentity.Service.Controllers
{
    [ApiController]
    public class AuthorizationController: ControllerBase
    {
        private readonly IMediator fMediator;

        public AuthorizationController(IMediator mediator)
        {
            fMediator = mediator;
        }

        [HttpPost(OpenIdConstants._TokenRoute), Produces("application/json")]
        public async Task<IActionResult> ConnectTokenAsync(CancellationToken cancellationToken)
        {
            var request = HttpContext.GetOpenIddictServerRequest();
            if (request == null)
            {
                throw new InvalidOperationException("Unable to retrieve OpenIddictServerRequest from HttpContext.");
            }

            var principal = await fMediator.Send(new TokenCommand(request), cancellationToken);
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
    }
}