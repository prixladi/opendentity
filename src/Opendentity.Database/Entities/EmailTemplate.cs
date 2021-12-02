using System;

namespace Opendentity.Database.Entities;

public class EmailTemplate
{
    public EmailTemplate()
    {
        Id = Guid.NewGuid().ToString();
    }

    public string Id { get; set; } = default!;
    public EmailTemplateType? Type { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string ContentTemplate { get; set; } = default!;
    public bool IsHtml { get; set; }
}
