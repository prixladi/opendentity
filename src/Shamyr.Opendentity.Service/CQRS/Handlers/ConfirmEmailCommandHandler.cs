using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shamyr.Exceptions;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.OpenId.Extensions;
using Shamyr.Opendentity.Service.CQRS.Commands;
using Shamyr.Opendentity.Service.CQRS.Handlers.Base;

namespace Shamyr.Opendentity.Service.CQRS.Handlers
{
    public class ConfirmEmailCommandHandler: EmailRequestHandlerBase, IRequestHandler<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandHandler(UserManager<ApplicationUser> userManager)
            : base(userManager) { }

        public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await GetByEmailOrThrowAsync(request.Email);
            var result = await userManager.ConfirmEmailAsync(user, request.Model.Token);
            if (!result.Succeeded)
                throw new IdentityException(result);

            return Unit.Value;
        }
    }
}
