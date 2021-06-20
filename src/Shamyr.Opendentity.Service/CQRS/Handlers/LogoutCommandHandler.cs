using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shamyr.Exceptions;
using Shamyr.Opendentity.OpenId.Services;
using Shamyr.Opendentity.Service.CQRS.Commands;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Shamyr.Opendentity.Service.CQRS.Handlers
{
    public class LogoutCommandHandler: IRequestHandler<LogoutCommand>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ISubjectTokenRevokationService subjectTokenRevokationService;

        public LogoutCommandHandler(IHttpContextAccessor httpContextAccessor, ISubjectTokenRevokationService subjectTokenRevokationService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.subjectTokenRevokationService = subjectTokenRevokationService;
        }

        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var userId = httpContextAccessor.HttpContext!.User.FindFirst(Claims.Subject)?.Value;
            if (userId is null)
                throw new BadRequestException("Unable to logout user that is not logged in.");

            await subjectTokenRevokationService.RevokeAllAsync(userId, cancellationToken);

            return Unit.Value;
        }
    }
}
