using OpenIddict.Server;

namespace Shamyr.Opendentity.OpenId
{
    public class OpenIdSettings: OpenIddictServerOptions
    {
        public bool RequireConfirmedAccount { get; init; }
    }
}
