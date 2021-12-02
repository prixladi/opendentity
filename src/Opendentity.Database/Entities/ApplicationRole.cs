using Microsoft.AspNetCore.Identity;

namespace Opendentity.Database.Entities;

public class ApplicationRole: IdentityRole
{
    public ApplicationRole() { }

    public ApplicationRole(string roleName)
        : base(roleName) { }
}
