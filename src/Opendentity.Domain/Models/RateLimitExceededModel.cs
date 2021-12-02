namespace Opendentity.Domain.Models;

public record RateLimitExceededModel
{
    public string Message { get; init; } = default!;
}
