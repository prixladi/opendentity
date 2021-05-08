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
    public class SetCurrentUserPasswordCommandHandler: IRequestHandler<SetCurrentUserPasswordCommand>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;

        public SetCurrentUserPasswordCommandHandler(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(SetCurrentUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext!.User);

            if (await userManager.HasPasswordAsync(user))
                throw new ConflictException($"Current user has password set.");

            var result = await userManager.AddPasswordAsync(user, request.Model.Password);
            if (!result.Succeeded)
                throw new BadRequestException(result.ToString());

            return Unit.Value;
        }
    }
}
