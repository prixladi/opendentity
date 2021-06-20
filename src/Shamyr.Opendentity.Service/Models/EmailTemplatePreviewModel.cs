using Shamyr.Opendentity.Database.Entities;

namespace Shamyr.Opendentity.Service.Models
{
    public record EmailTemplatePreviewModel
    {
        public string Id { get; init; } = default!;
        public EmailTemplateType? Type { get; init; }
        public string Subject { get; init; } = default!;
        public bool IsHtml { get; init; }
    }
}
