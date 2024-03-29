﻿using Opendentity.Database.Entities;

namespace Opendentity.Domain.Services;

public interface IEmailTemplateCache
{
    Task AddOrUpdateAsync(EmailTemplateType type, EmailTemplate template, CancellationToken cancellationToken);
    Task RemoveAsync(EmailTemplateType type, CancellationToken cancellationToken);
    Task<EmailTemplate?> TryGetAsync(EmailTemplateType type, CancellationToken cancellationToken);
    Task ClearAsync(CancellationToken cancellationToken);
}
