namespace Opendentity.Service.DatabaseInit;

public record ApplicationInitDto
{
    public string ClientId { get; init; } = default!;
}
