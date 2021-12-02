namespace Opendentity.Service.Models;

public record CreateUserModel
{
    public string UserName { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
    public string? FirstName { get; init; }
    public string? LastName { get; init; } = default!;
    public string? ImageUrl { get; init; } = default!;
}
