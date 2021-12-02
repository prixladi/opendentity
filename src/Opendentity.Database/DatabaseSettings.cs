namespace Opendentity.Database;

public record DatabaseSettings
{
    public virtual string ConnectionString { get; init; } = default!;
}
