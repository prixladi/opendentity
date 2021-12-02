namespace Opendentity.Service.DatabaseInit;

public record UserInitDto
{
    public string UserName { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
    public bool IsAdmin { get; init; }
    public string? FirstName { get; init; } = default!;
    public string? LastName { get; init; } = default!;
    public string? ImageUrl { get; init; } = default!;
}
