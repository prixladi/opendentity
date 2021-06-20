namespace Shamyr.Opendentity.Service.Models
{
    public record RateLimitExceededModel
    {
        public string Message { get; init; } = default!;
    }
}
