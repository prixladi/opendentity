using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Opendentity.Database.Entities;
using Shamyr.Exceptions;
using Opendentity.OpenId.Extensions;
using Opendentity.OpenId.Services;
using Opendentity.Service.CQRS.Commands;

namespace Opendentity.Service.CQRS.Handlers;

public class UpdateCurrentUserCommandHandler: IRequestHandler<UpdateCurrentUserCommand>
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IUserValidationService userValidationService;

    public UpdateCurrentUserCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager,
        IUserValidationService userValidationService)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.userManager = userManager;
        this.userValidationService = userValidationService;
    }

    public async Task<Unit> Handle(UpdateCurrentUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext!.User);
        userValidationService.ValidateUsernameOrThrow(request.Model.UserName);
        if (user.UserName != request.Model.UserName && (await userManager.FindByNameAsync(request.Model.UserName)) != null)
            throw new ConflictException($"User with username '{request.Model.UserName}' already exist.");

        user.UserName = request.Model.UserName;
        user.FirstName = request.Model.FirstName;
        user.LastName = request.Model.LastName;
        user.ImageUrl = request.Model.ImageUrl;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new IdentityException(result);

        return Unit.Value;
    }
}
