using System;
using OpenIddict.EntityFrameworkCore.Models;

namespace Opendentity.Database.Entities;

public class Application: OpenIddictEntityFrameworkCoreApplication<string, Authorization, Token>
{
    public Application()
    {
        Id = Guid.NewGuid().ToString();
    }
}
