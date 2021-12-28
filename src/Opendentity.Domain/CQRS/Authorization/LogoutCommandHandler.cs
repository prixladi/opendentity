using MediatR;
using Microsoft.AspNetCore.Http;
using Opendentity.Domain.RequestPipeline;
using Opendentity.OpenId.Services;
using Shamyr.Exceptions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Opendentity.Domain.CQRS.Authorization;

public record LogoutCommand: IRequest, ITransactionRequest;

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
        string? userId = httpContextAccessor.HttpContext!.User.FindFirst(Claims.Subject)?.Value;
        if (userId is null)
            throw new BadRequestException("Unable to logout user that is not logged in.");

        await subjectTokenRevokationService.RevokeAllAsync(userId, cancellationToken);

        return Unit.Value;
    }
}
