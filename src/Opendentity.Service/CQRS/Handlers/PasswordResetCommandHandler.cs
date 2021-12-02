using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Opendentity.Database.Entities;
using Opendentity.OpenId.Extensions;
using Opendentity.OpenId.Services;
using Opendentity.Service.CQRS.Commands;
using Opendentity.Service.CQRS.Handlers.Base;

namespace Opendentity.Service.CQRS.Handlers;

public class PasswordResetCommandHandler: EmailRequestHandlerBase, IRequestHandler<PasswordResetCommand>
{
    private readonly ISubjectTokenRevokationService subjectTokenRevokationService;

    public PasswordResetCommandHandler(UserManager<ApplicationUser> userManager, ISubjectTokenRevokationService subjectTokenRevokationService)
        : base(userManager)
    {
        this.subjectTokenRevokationService = subjectTokenRevokationService;
    }

    public async Task<Unit> Handle(PasswordResetCommand request, CancellationToken cancellationToken)
    {
        var user = await GetByEmailOrThrowAsync(request.Email);
        var result = await userManager.ResetPasswordAsync(user, request.Model.Token, request.Model.Password);
        if (!result.Succeeded)
            throw new IdentityException(result);

        await subjectTokenRevokationService.RevokeAllAsync(user.Id, cancellationToken);

        return Unit.Value;
    }
}
