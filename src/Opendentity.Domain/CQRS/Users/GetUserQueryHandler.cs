using MediatR;
using Microsoft.AspNetCore.Identity;
using Opendentity.Database.Entities;
using Opendentity.Domain.Extensions;
using Opendentity.Domain.Models;
using Shamyr.Exceptions;

namespace Opendentity.Domain.CQRS.Users;

public record GetUserQuery(string Id): IRequest<UserModel>;

public class GetUserQueryHandler: IRequestHandler<GetUserQuery, UserModel>
{
    private readonly UserManager<ApplicationUser> userManager;

    public GetUserQueryHandler(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }

    public async Task<UserModel> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id);
        if (user == null)
            throw new NotFoundException($"User with ID '{request.Id}' not found.");

        return user.ToModel();
    }
}
