namespace Opendentity.Domain.Models;

public record EmailTemplateModel: EmailTemplatePreviewModel
{
    public string ContentTemplate { get; init; } = default!;
}
