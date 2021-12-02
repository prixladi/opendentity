namespace Opendentity.Domain.DatabaseInit;

public record DatabaseInitSettings
{
    public string? InitFilePath { get; init; }
}
