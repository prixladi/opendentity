namespace Opendentity.Domain.Settings;

public record UISettings
{
    public Uri PortalUrl { get; init; } = default!;
}
