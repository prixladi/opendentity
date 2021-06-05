using System.ComponentModel.DataAnnotations;
using Shamyr.Opendentity.Database.Entities;

namespace Shamyr.Opendentity.Service.Models
{
    public record EmailTemplatePreviewModel
    {
        [Required]
        public string Id { get; init; } = default!;
        public EmailTemplateType? Type { get; init; } 

        [Required]
        public string Subject { get; init; } = default!;
        public bool IsHtml { get; init; }
    }
}
