using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Opendentity.Database.Entities;
using Opendentity.Domain.Models;
using Opendentity.Domain.RequestPipeline;
using Opendentity.OpenId.Extensions;
using Opendentity.OpenId.Services;
using Shamyr.Exceptions;

namespace Opendentity.Domain.CQRS.Users;

public record UpdateUserEmailConfirmedCommand(string Id, UpdateFlagModel Model): IRequest, ITransactionRequest;

public class UpdateUserEmailConfirmedCommandHandler: IRequestHandler<UpdateUserEmailConfirmedCommand>
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly ISubjectTokenRevokationService subjectTokenRevokationService;
    private readonly IOptions<IdentityOptions> options;

    public UpdateUserEmailConfirmedCommandHandler(
        UserManager<ApplicationUser> userManager, 
        ISubjectTokenRevokationService subjectTokenRevokationService,
        IOptions<IdentityOptions> options)
    {
        this.userManager = userManager;
        this.subjectTokenRevokationService = subjectTokenRevokationService;
        this.options = options;
    }

    public async Task<Unit> Handle(UpdateUserEmailConfirmedCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id);
        if (user == null)
            throw new NotFoundException($"User with ID '{request.Id}' not found.");

        if (user.EmailConfirmed == request.Model.Value)
            return Unit.Value;

        user.EmailConfirmed = request.Model.Value;
        if (user.Disabled && options.Value.SignIn.RequireConfirmedAccount)
            await subjectTokenRevokationService.RevokeAllAsync(user.Id, cancellationToken);

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new IdentityException(result);

        return Unit.Value;
    }
}
