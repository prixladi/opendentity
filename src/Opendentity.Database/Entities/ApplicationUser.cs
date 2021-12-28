using Microsoft.AspNetCore.Identity;

namespace Opendentity.Database.Entities;

public class ApplicationUser: IdentityUser
{
    public ApplicationUser(string userName)
        : base(userName) { }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }
    public bool Disabled { get; set; }
}
