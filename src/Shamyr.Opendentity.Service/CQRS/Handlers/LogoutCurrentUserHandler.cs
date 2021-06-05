using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shamyr.Opendentity.OpenId.Services;
using Shamyr.Opendentity.Service.CQRS.Commands;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Shamyr.Opendentity.Service.CQRS.Handlers
{
    public class LogoutCurrentUserHandler: IRequestHandler<LogoutCurrentUser>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ISubjectTokenRevokationService subjectTokenRevokationService;

        public LogoutCurrentUserHandler(IHttpContextAccessor httpContextAccessor, ISubjectTokenRevokationService subjectTokenRevokationService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.subjectTokenRevokationService = subjectTokenRevokationService;
        }

        public async Task<Unit> Handle(LogoutCurrentUser request, CancellationToken cancellationToken)
        {
            var userId = httpContextAccessor.HttpContext!.User.FindFirst(Claims.Subject)?.Value;
            if (userId is null)
                throw new InvalidOperationException("Unable to retrieve current user id");

            await subjectTokenRevokationService.RevokeAllAsync(userId, cancellationToken);

            return Unit.Value;
        }
    }
}
