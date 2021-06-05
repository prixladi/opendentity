using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shamyr.Opendentity.Database.Entities;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Shamyr.Opendentity.OpenId.Factories
{
    public class ClaimsPrincipalFactory: UserClaimsPrincipalFactory<ApplicationUser>
    {
        public ClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor) { }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            if (!string.IsNullOrEmpty(user.FirstName))
                identity.AddClaim(new Claim(Claims.GivenName, user.FirstName));

            if (!string.IsNullOrEmpty(user.LastName))
                identity.AddClaim(new Claim(Claims.FamilyName, user.LastName));

            if (!string.IsNullOrEmpty(user.ImageUrl))
                identity.AddClaim(new Claim(Claims.Picture, user.ImageUrl));

            foreach (var role in await UserManager.GetRolesAsync(user))
                identity.AddClaim(new Claim(Claims.Role, role));

            return identity;
        }
    }
}
