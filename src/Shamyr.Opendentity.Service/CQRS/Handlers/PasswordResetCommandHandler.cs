using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shamyr.Exceptions;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.Service.CQRS.Commands;

namespace Shamyr.Opendentity.Service.CQRS.Handlers
{
    public record PasswordResetCommandHandler: IRequestHandler<PasswordResetCommand>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public PasswordResetCommandHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(PasswordResetCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new NotFoundException($"User with Email '{request.Email}' not found.");

            var result = await userManager.ResetPasswordAsync(user, request.Model.Token, request.Model.Password);
            if (!result.Succeeded)
                throw new BadRequestException(result.ToString());

            return Unit.Value;

        }
    }
}
