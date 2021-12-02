namespace Opendentity.Domain.Models;

public record UsersFilterModel
{
    public int Offset { get; init; }
    public int Limit { get; init; }
    public string? Search { get; init; }
}
