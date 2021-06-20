using Shamyr.Opendentity.Database.Entities;

namespace Shamyr.Opendentity.Service.Models
{
    public record CreateEmailTemplateModel
    {
        public EmailTemplateType? Type { get; init; } = default!;
        public string Subject { get; init; } = default!;
        public string ContentTemplate { get; init; } = default!;
        public bool IsHtml { get; init; } = default!;
    }
}
