using System.Collections.Generic;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Shamyr.Opendentity.Service.OpenId
{
    public static class Utils
    {
        public static IEnumerable<string> GetClaimDestinations(Claim claim)
        {
            switch (claim.Type)
            {
                case "AspNet.Identity.SecurityStamp":
                    yield break;

                default:
                    yield return Destinations.AccessToken;
                    yield break;
            }
        }
    }
}
