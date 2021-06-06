using System.Collections.Generic;
using System.Security.Claims;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Shamyr.Opendentity.OpenId
{
    internal static class Utils
    {
        public const string _AllowedUsernameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@-";

        public static IEnumerable<string> GetClaimDestinations(Claim claim, ClaimsPrincipal principal)
        {
            switch (claim.Type)
            {
                case Claims.Email when principal.HasScope(Scopes.Email):
                case Claims.EmailVerified when principal.HasScope(Scopes.Email):
                    yield return Destinations.AccessToken;
                    yield return Destinations.IdentityToken;
                    yield break;

                case Claims.GivenName when principal.HasScope(Scopes.Profile):
                case Claims.FamilyName when principal.HasScope(Scopes.Profile):
                case Claims.MiddleName when principal.HasScope(Scopes.Profile):
                case Claims.Username when principal.HasScope(Scopes.Profile):
                case Claims.Name when principal.HasScope(Scopes.Profile):
                case Claims.Picture when principal.HasScope(Scopes.Profile):
                    yield return Destinations.AccessToken;
                    yield return Destinations.IdentityToken;
                    yield break;

                case Claims.PhoneNumber when principal.HasScope(Scopes.Phone):
                case Claims.PhoneNumberVerified when principal.HasScope(Scopes.Phone):
                    yield return Destinations.AccessToken;
                    yield return Destinations.IdentityToken;
                    yield break;

                case Claims.Role when principal.HasScope(Scopes.Roles):
                    yield return Destinations.AccessToken;
                    yield return Destinations.IdentityToken;
                    yield break;

                case "AspNet.Identity.SecurityStamp":
                    yield break;

                default:
                    yield return Destinations.IdentityToken;
                    yield break;
            }
        }
    }
}
