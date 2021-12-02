using MediatR;
using Microsoft.AspNetCore.Identity;
using Opendentity.Database.Entities;
using Opendentity.Domain.CQRS.Base;
using Opendentity.Domain.Models;
using Opendentity.OpenId.Extensions;

namespace Opendentity.Domain.CQRS.Emails;

public record ConfirmEmailCommand(string Email, TokenModel Model): IRequest;

public class ConfirmEmailCommandHandler: EmailRequestHandlerBase, IRequestHandler<ConfirmEmailCommand>
{
    public ConfirmEmailCommandHandler(UserManager<ApplicationUser> userManager)
        : base(userManager) { }

    public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await GetByEmailOrThrowAsync(request.Email);
        var result = await userManager.ConfirmEmailAsync(user, request.Model.Token);
        if (!result.Succeeded)
            throw new IdentityException(result);

        return Unit.Value;
    }
}
