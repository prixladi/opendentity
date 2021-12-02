﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Opendentity.Database.Entities;
using Opendentity.Service.CQRS.Queries;
using Opendentity.Service.Extensions;
using Opendentity.Service.Models;

namespace Opendentity.Service.CQRS.Handlers;

public class GetCurrentUserQueryHandler: IRequestHandler<GetCurrentUserQuery, UserModel>
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly UserManager<ApplicationUser> userManager;

    public GetCurrentUserQueryHandler(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.userManager = userManager;
    }

    public async Task<UserModel> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var principal = httpContextAccessor.HttpContext!.User;
        var user = await userManager.GetUserAsync(principal);
        if (user is null)
            throw new Exception("Unable to retrieve current user from manager.");

        return user.ToModel();
    }
}
