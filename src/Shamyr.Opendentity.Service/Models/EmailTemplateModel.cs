using System.ComponentModel.DataAnnotations;

namespace Shamyr.Opendentity.Service.Models
{
    public record EmailTemplateModel: EmailTemplatePreviewModel
    {
        public string ContentTemplate { get; init; } = default!;
    }
}
