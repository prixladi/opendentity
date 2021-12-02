using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Opendentity.Database.Entities;
using Opendentity.Domain.Models;
using Opendentity.Domain.RequestPipeline;
using Opendentity.OpenId.Extensions;
using Opendentity.OpenId.Services;
using Shamyr.Exceptions;

namespace Opendentity.Domain.CQRS;

public record SetCurrentUserPasswordCommand(SetPasswordModel Model): IRequest, ITransactionRequest;

public class SetCurrentUserPasswordCommandHandler: IRequestHandler<SetCurrentUserPasswordCommand>
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly ISubjectTokenRevokationService subjectTokenRevokationService;

    public SetCurrentUserPasswordCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager,
        ISubjectTokenRevokationService subjectTokenRevokationService)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.userManager = userManager;
        this.subjectTokenRevokationService = subjectTokenRevokationService;
    }

    public async Task<Unit> Handle(SetCurrentUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext!.User);

        if (await userManager.HasPasswordAsync(user))
            throw new ConflictException($"Current user has password set.");

        var result = await userManager.AddPasswordAsync(user, request.Model.Password);
        if (!result.Succeeded)
            throw new IdentityException(result);

        await subjectTokenRevokationService.RevokeAllAsync(user.Id, cancellationToken);

        return Unit.Value;
    }
}
