using Opendentity.Database.Entities;

namespace Opendentity.Domain.Services;

public class EmailTemplateCache: IEmailTemplateCache
{
    public Task<EmailTemplate?> TryGetAsync(EmailTemplateType type, CancellationToken cancellationToken)
    {
        return Task.FromResult<EmailTemplate?>(null);
    }

    public Task AddOrUpdateAsync(EmailTemplateType type, EmailTemplate template, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task RemoveAsync(EmailTemplateType type, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task ClearAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
