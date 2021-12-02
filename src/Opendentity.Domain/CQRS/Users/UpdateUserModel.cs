namespace Opendentity.Domain.Models;

public record UpdateUserModel
{
    public string UserName { get; init; } = default!;

    public string? FirstName { get; init; }
    public string? LastName { get; init; } = default!;
    public string? ImageUrl { get; init; } = default!;
}
