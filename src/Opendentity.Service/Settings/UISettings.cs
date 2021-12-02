using System;

namespace Opendentity.Service.Configs;

public record UISettings
{
    public Uri PortalUrl { get; init; } = default!;
}
