using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shamyr.Exceptions;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.OpenId.Services;
using Shamyr.Opendentity.Service.CQRS.Commands;
using Shamyr.Opendentity.Service.CQRS.Handlers.Base;

namespace Shamyr.Opendentity.Service.CQRS.Handlers
{
    public class PasswordResetCommandHandler: EmailRequestHandlerBase, IRequestHandler<PasswordResetCommand>
    {
        private readonly ISubjectTokenRevokationService subjectTokenRevokationService;

        public PasswordResetCommandHandler(UserManager<ApplicationUser> userManager, ISubjectTokenRevokationService subjectTokenRevokationService)
            : base(userManager)
        {
            this.subjectTokenRevokationService = subjectTokenRevokationService;
        }

        public async Task<Unit> Handle(PasswordResetCommand request, CancellationToken cancellationToken)
        {
            var user = await GetByEmailOrThrowAsync(request.Email);
            var result = await userManager.ResetPasswordAsync(user, request.Model.Token, request.Model.Password);
            if (!result.Succeeded)
                throw new BadRequestException(result.ToString());

            await subjectTokenRevokationService.RevokeAllAsync(user.Id, cancellationToken);

            return Unit.Value;
        }
    }
}
