using System;
using OpenIddict.EntityFrameworkCore.Models;

namespace Opendentity.Database.Entities;

public class Authorization: OpenIddictEntityFrameworkCoreAuthorization<string, Application, Token>
{
    public Authorization()
    {
        Id = Guid.NewGuid().ToString();
    }
}
