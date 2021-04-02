using Microsoft.AspNetCore.Identity;

namespace Shamyr.Opendentity.Database.Entities
{
    public class ApplicationRole: IdentityRole
    {
        public ApplicationRole() { }

        public ApplicationRole(string roleName)
            : base(roleName) { }
    }
}
