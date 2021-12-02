using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Opendentity.Database.Entities;
using Shamyr.Exceptions;

namespace Opendentity.Service.CQRS.Handlers.Base;

public abstract class EmailRequestHandlerBase
{
    protected readonly UserManager<ApplicationUser> userManager;

    protected EmailRequestHandlerBase(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }

    protected async Task<ApplicationUser> GetByEmailOrThrowAsync(string email)
    {
        if (email is null)
            throw new ArgumentNullException(nameof(email));

        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            throw new NotFoundException($"User with email '{email}' not found.");

        return user;
    }
}
