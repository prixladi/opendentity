using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Shamyr.Exceptions;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.Service.CQRS.Commands;

namespace Shamyr.Opendentity.Service.CQRS.Handlers
{
    public class UpdateCurrentUserCommandHandler: IRequestHandler<UpdateCurrentUserCommand>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;

        public UpdateCurrentUserCommandHandler(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(UpdateCurrentUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext!.User);

            if (user.UserName != request.Model.UserName && (await userManager.FindByNameAsync(request.Model.UserName)) != null)
                throw new ConflictException($"User with username '{request.Model.UserName}' already exist.");

            if (user.Email != request.Model.Email && (await userManager.FindByEmailAsync(request.Model.Email)) != null)
                throw new ConflictException($"User with email '{request.Model.Email}' already exist.");

            user.UserName = request.Model.UserName;
            user.Email = request.Model.Email;
            user.FirstName = request.Model.FirstName;
            user.LastName = request.Model.LastName;
            user.ImageUrl = request.Model.ImageUrl;

            await userManager.UpdateAsync(user);

            return Unit.Value;
        }
    }
}
