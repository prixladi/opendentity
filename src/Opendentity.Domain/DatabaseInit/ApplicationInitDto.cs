namespace Opendentity.Domain.DatabaseInit;

public record ApplicationInitDto
{
    public string ClientId { get; init; } = default!;
}
