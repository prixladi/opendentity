using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.Service.CQRS.Queries;
using Shamyr.Opendentity.Service.Extensions;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.CQRS.Handlers
{
    public class GetUsersQueryHandler: IRequestHandler<GetUsersQuery, UsersModel>
    {
        private readonly UserManager<ApplicationUser> manager;

        public GetUsersQueryHandler(UserManager<ApplicationUser> manager)
        {
            this.manager = manager;
        }

        public async Task<UsersModel> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var query = request.Model.Search is null ? manager.Users : manager.Users
                .Where(e => EF.Functions.ToTsVector("english", e.UserName + " " + e.Email + " " + e.FirstName + " " + e.LastName)
                    .Matches(request.Model.Search));

            var count = await query.CountAsync(cancellationToken);

            var data = await query
                .Skip(request.Model.Offset)
                .Take(request.Model.Limit)
                .ToArrayAsync(cancellationToken);

            return new UsersModel
            {
                Total = count,
                Users = data.ToModel()
            };
        }
    }
}
