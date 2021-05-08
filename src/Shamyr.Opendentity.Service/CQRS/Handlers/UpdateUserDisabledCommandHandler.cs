using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shamyr.Exceptions;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.Service.CQRS.Commands;

namespace Shamyr.Opendentity.Service.CQRS.Handlers
{
    public class UpdateUserDisabledCommandHandler: IRequestHandler<UpdateUserDisabledCommand>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UpdateUserDisabledCommandHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(UpdateUserDisabledCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.Id);
            if (user == null)
                throw new NotFoundException($"User with ID '{request.Id}' not found.");

            user.Disabled = request.Model.Disabled;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new BadRequestException(result.ToString());

            return Unit.Value;
        }
    }
}
