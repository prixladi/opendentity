using System.ComponentModel.DataAnnotations;
using Shamyr.Opendentity.Database.Entities;

namespace Shamyr.Opendentity.Service.Models
{
    public record UpdateEmailTemplateModel
    {
        public EmailTemplateType? Type { get; init; } = default!;

        [Required]
        public string Subject { get; init; } = default!;

        [Required]
        public string ContentTemplate { get; init; } = default!;

        public bool IsHtml { get; init; } = default!;
    }
}
