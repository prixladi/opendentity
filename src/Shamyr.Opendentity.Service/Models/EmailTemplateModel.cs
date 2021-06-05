using System.ComponentModel.DataAnnotations;

namespace Shamyr.Opendentity.Service.Models
{
    public record EmailTemplateModel: EmailTemplatePreviewModel
    {
        [Required]
        public string ContentTemplate { get; init; } = default!;
    }
}
