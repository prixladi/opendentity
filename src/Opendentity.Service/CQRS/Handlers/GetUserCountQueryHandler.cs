using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Opendentity.Database.Entities;
using Opendentity.Service.CQRS.Queries;
using Opendentity.Service.Models;

namespace Opendentity.Service.CQRS.Handlers;

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
