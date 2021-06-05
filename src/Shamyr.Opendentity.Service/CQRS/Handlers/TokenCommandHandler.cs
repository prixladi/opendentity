using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shamyr.Opendentity.OpenId.Factories;
using Shamyr.Opendentity.Service.CQRS.Commands;

namespace Shamyr.Opendentity.Service.CQRS.Handlers
{
    public class TokenCommandHandler: IRequestHandler<TokenCommand, ClaimsPrincipal>
    {
        private readonly IGrantHandlerFactory grantHandlerFactory;

        public TokenCommandHandler(IGrantHandlerFactory grantHandlerFactory)
        {
            this.grantHandlerFactory = grantHandlerFactory;
        }

        public Task<ClaimsPrincipal> Handle(TokenCommand request, CancellationToken cancellationToken)
        {
            var validator = grantHandlerFactory.Create(request.OpenIddictRequest);
            if (validator == null)
                throw new NotImplementedException($"Flow for grat of type '{request.OpenIddictRequest.GrantType}' is not implemented");

            return validator.HandleAsync(request.OpenIddictRequest, cancellationToken);
        }
    }
}
