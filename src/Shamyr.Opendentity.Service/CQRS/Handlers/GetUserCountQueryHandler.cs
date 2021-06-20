using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.Service.CQRS.Queries;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.CQRS.Handlers
{
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
}
