using System.Security.Claims;
using MediatR;
using Opendentity.OpenId.Factories;
using OpenIddict.Abstractions;

namespace Opendentity.Domain.CQRS.Authorization;

public record TokenCommand(OpenIddictRequest OpenIddictRequest): IRequest<ClaimsPrincipal>;

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
