namespace Opendentity.Domain.Models;

public class UserModel
{
    public string Id { get; init; } = default!;
    public string UserName { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string? FirstName { get; init; }
    public string? LastName { get; init; } = default!;
    public string? ImageUrl { get; init; } = default!;
    public bool EmailConfirmed { get; init; }
}
