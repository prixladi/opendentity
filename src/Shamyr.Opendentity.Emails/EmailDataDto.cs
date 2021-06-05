namespace Shamyr.Opendentity.Emails
{
    public record EmailDataDto
    {
        public EmailBodyDto Body { get; init; } = default!;
        public string Subject { get; init; } = default!;
    }
}
