namespace Shamyr.Opendentity.Service.DatabaseInit
{
    public record EmailTemplateDto
    {
        public string ContentTemplate { get; init; } = default!;
        public string Subject { get; init; } = default!;
        public bool IsHtml { get; init; }
    }
}
