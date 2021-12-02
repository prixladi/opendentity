﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Opendentity.Database.Entities;
using Shamyr.Exceptions;
using Opendentity.OpenId.Extensions;
using Opendentity.OpenId.Services;
using Opendentity.Service.CQRS.Commands;

namespace Opendentity.Service.CQRS.Handlers;

public class ChangeCurrentUserPasswordCommandHandler: IRequestHandler<ChangeCurrentUserPasswordCommand>
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly ISubjectTokenRevokationService subjectTokenRevokationService;

    public ChangeCurrentUserPasswordCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager,
        ISubjectTokenRevokationService subjectTokenRevokationService)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.userManager = userManager;
        this.subjectTokenRevokationService = subjectTokenRevokationService;
    }

    public async Task<Unit> Handle(ChangeCurrentUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext!.User);
        if (!await userManager.HasPasswordAsync(user))
            throw new ConflictException($"Current user doesn't have password set.");

        var result = await userManager.ChangePasswordAsync(user, request.Model.OldPassword, request.Model.NewPassword);
        if (!result.Succeeded)
            throw new IdentityException(result);

        await subjectTokenRevokationService.RevokeAllAsync(user.Id, cancellationToken);

        return Unit.Value;
    }
}
