using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shamyr.Opendentity.Service.OpenId.Factories;
using Shamyr.Opendentity.Service.Requests.Commands;

namespace Shamyr.Opendentity.Service.Handlers.Commands
{
    public class TokenCommandHandler: IRequestHandler<TokenCommand, ClaimsPrincipal>
    {
        private readonly IGrantHandlerFactory fGrantHandlerFactory;

        public TokenCommandHandler(IGrantHandlerFactory grantHandlerFactory)
        {
            fGrantHandlerFactory = grantHandlerFactory;
        }

        public Task<ClaimsPrincipal> Handle(TokenCommand request, CancellationToken cancellationToken)
        {
            var validator = fGrantHandlerFactory.Create(request.OpenIddictRequest);
            if (validator == null)
            {
                throw new NotImplementedException($"Flow for grat of type '{request.OpenIddictRequest.GrantType}' is not implemented");
            }

            return validator.HandleAsync(request.OpenIddictRequest, cancellationToken);
        }
    }
}
