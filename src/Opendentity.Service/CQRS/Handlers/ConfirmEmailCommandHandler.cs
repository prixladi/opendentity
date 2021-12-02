using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Opendentity.Database.Entities;
using Opendentity.OpenId.Extensions;
using Opendentity.Service.CQRS.Commands;
using Opendentity.Service.CQRS.Handlers.Base;

namespace Opendentity.Service.CQRS.Handlers;

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
