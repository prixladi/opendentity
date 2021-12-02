using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Opendentity.Database.Entities;
using Shamyr.Exceptions;
using Opendentity.Service.CQRS.Queries;
using Opendentity.Service.Extensions;
using Opendentity.Service.Models;

namespace Opendentity.Service.CQRS.Handlers;

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
