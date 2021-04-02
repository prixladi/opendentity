using Microsoft.AspNetCore.Identity;

namespace Shamyr.Opendentity.Database.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ImageUrl { get; set; }
    }
}
