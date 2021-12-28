namespace Opendentity.Domain.Models;

public record SetPasswordModel
{
    public string Password { get; init; } = default!;
}
