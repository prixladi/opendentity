using MediatR;
using Microsoft.AspNetCore.Identity;
using Opendentity.Database.Entities;
using Opendentity.Domain.Models;
using Opendentity.Domain.RequestPipeline;
using Opendentity.OpenId.Extensions;
using Opendentity.OpenId.Services;
using Shamyr.Exceptions;

namespace Opendentity.Domain.CQRS;

public record UpdateUserDisabledCommand(string Id, UpdateDisabledModel Model): IRequest, ITransactionRequest;

public class UpdateUserDisabledCommandHandler: IRequestHandler<UpdateUserDisabledCommand>
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly ISubjectTokenRevokationService subjectTokenRevokationService;

    public UpdateUserDisabledCommandHandler(UserManager<ApplicationUser> userManager, ISubjectTokenRevokationService subjectTokenRevokationService)
    {
        this.userManager = userManager;
        this.subjectTokenRevokationService = subjectTokenRevokationService;
    }

    public async Task<Unit> Handle(UpdateUserDisabledCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id);
        if (user == null)
            throw new NotFoundException($"User with ID '{request.Id}' not found.");

        if (user.Disabled == request.Model.Disabled)
            return Unit.Value;

        user.Disabled = request.Model.Disabled;
        if (user.Disabled)
            await subjectTokenRevokationService.RevokeAllAsync(user.Id, cancellationToken);

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new IdentityException(result);

        return Unit.Value;
    }
}
