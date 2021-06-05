using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shamyr.Exceptions;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.Service.CQRS.Queries;
using Shamyr.Opendentity.Service.Extensions;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.CQRS.Handlers
{
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
}
