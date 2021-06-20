using System;

namespace Shamyr.Opendentity.Service.Configs
{
    public record UISettings
    {
        public Uri PortalUrl { get; init; } = default!;
    }
}
