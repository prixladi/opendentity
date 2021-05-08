using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shamyr.Exceptions;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.Service.CQRS.Commands;

namespace Shamyr.Opendentity.Service.CQRS.Handlers
{
    public class SendPasswordResetCommandHandler: IRequestHandler<SendPasswordResetCommand>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public SendPasswordResetCommandHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(SendPasswordResetCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new NotFoundException($"User with Email '{request.Email}' not found.");

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            throw new NotImplementedException(token);
        }
    }
}
