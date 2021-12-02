using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Opendentity.Database.Entities;
using Opendentity.Domain.Models;

namespace Opendentity.Domain.CQRS;

public record GetUserCountQuery: IRequest<CountModel>;

public class GetUserCountQueryHandler: IRequestHandler<GetUserCountQuery, CountModel>
{
    private readonly UserManager<ApplicationUser> manager;

    public GetUserCountQueryHandler(UserManager<ApplicationUser> manager)
    {
        this.manager = manager;
    }

    public async Task<CountModel> Handle(GetUserCountQuery request, CancellationToken cancellationToken)
    {
        var count = await manager.Users.CountAsync(cancellationToken);

        return new CountModel
        {
            Count = count
        };
    }
}
