namespace Opendentity.Domain.Models;

public record PasswordResetModel
{
    public string Token { get; init; } = default!;
    public string Password { get; init; } = default!;
}
