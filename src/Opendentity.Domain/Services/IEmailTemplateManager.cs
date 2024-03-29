﻿using Opendentity.Database.Entities;

namespace Opendentity.Domain.Services;

public interface IEmailTemplateManager
{
    Task<bool> RemoveEmailTemplateAsync(string key, CancellationToken cancellationToken);
    Task CreateEmailTemplateAsync(EmailTemplate dto, CancellationToken cancellationToken);
    Task<EmailTemplate?> TryGetTemplateAsync(string key, CancellationToken cancellationToken);
    Task UpdateEmailTemplateAsync(EmailTemplate template, EmailTemplateType? oldType, CancellationToken cancellationToken);
    Task<EmailTemplate?> TryGetTemplateAsync(EmailTemplateType type, CancellationToken cancellationToken);
}
