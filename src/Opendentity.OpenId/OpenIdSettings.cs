using OpenIddict.Server;

namespace Opendentity.OpenId;

public class OpenIdSettings: OpenIddictServerOptions
{
    public bool RequireConfirmedAccount { get; init; }
}
