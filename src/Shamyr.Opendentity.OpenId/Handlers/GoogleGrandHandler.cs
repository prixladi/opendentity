using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using Shamyr.Exceptions;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.OpenId.Extensions;

namespace Shamyr.Opendentity.OpenId.Handlers
{
    public class GoogleGrandHandler: IGrantHandler
    {
        private const string _IdTokenParameter = "id_token";

        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public GoogleGrandHandler(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public bool CanHandle(OpenIddictRequest request)
        {
            return request.GrantType == CustomGrants._Google;
        }

        public async Task<ClaimsPrincipal> HandleAsync(OpenIddictRequest request, CancellationToken cancellationToken)
        {
            if (!request.TryGetParameter(_IdTokenParameter, out var parameter))
                throw new BadRequestException("Id token is required for google grant.");

            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync((string)parameter!);
            }
            catch (InvalidJwtException)
            {
                throw new BadRequestException("Google token is invalid.");
            }

            var user = await userManager.FindByEmailAsync(payload.Email);
            if (user is null)
            {
                user = payload.ToUser();
                await userManager.CreateAsync(user);
            }
            else if (!user.EmailConfirmed)
            {
                // By logging with google we automaticaly validate user's email, hence confirmed
                user.EmailConfirmed = true;
                await userManager.UpdateAsync(user);
            }

            var principal = await signInManager.CreateUserPrincipalAsync(user);
            principal.SetScopes(request.GetScopes());

            foreach (var claim in principal.Claims)
                claim.SetDestinations(Utils.GetClaimDestinations(claim, principal));

            return principal;
        }
    }
}
