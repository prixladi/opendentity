using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Opendentity.Database.Entities;
using Opendentity.Domain.CQRS.Users;
using Opendentity.OpenId.Extensions;
using Shamyr.Exceptions;

namespace Opendentity.Domain.CQRS.CurrentUser;

public record UpdateCurrentUserCommand(UpdateUserModel Model): IRequest;

public class UpdateCurrentUserCommandHandler: IRequestHandler<UpdateCurrentUserCommand>
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly UserManager<ApplicationUser> userManager;

    public UpdateCurrentUserCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.userManager = userManager;
    }

    public async Task<Unit> Handle(UpdateCurrentUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext!.User);
        if (user.UserName != request.Model.UserName && await userManager.FindByNameAsync(request.Model.UserName) != null)
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
