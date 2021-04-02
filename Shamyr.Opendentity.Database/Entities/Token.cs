using System;
using OpenIddict.EntityFrameworkCore.Models;

namespace Shamyr.Opendentity.Database.Entities
{
    public class Token: OpenIddictEntityFrameworkCoreToken<string, Application, Authorization>
    {
        public Token()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
