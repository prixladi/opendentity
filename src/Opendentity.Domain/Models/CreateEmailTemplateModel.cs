﻿using Opendentity.Database.Entities;

namespace Opendentity.Domain.Models;

public record CreateEmailTemplateModel
{
    public EmailTemplateType? Type { get; init; } = default!;
    public string Subject { get; init; } = default!;
    public string ContentTemplate { get; init; } = default!;
    public bool IsHtml { get; init; } = default!;
}
