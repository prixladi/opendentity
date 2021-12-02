using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Opendentity.Database;
using Opendentity.Database.Entities;
using Shamyr.Threading.Async;

namespace Opendentity.Service.Services;

public class EmailTemplateManager: IEmailTemplateManager
{
    private readonly DatabaseContext databaseContext;
    private readonly IEmailTemplateCache cache;
    private readonly AsyncSemaphoreLock asyncLock;

    public EmailTemplateManager(DatabaseContext databaseContext, IEmailTemplateCache cache)
    {
        asyncLock = new AsyncSemaphoreLock();
        this.databaseContext = databaseContext;
        this.cache = cache;
    }

    public async Task<EmailTemplate?> TryGetTemplateAsync(string id, CancellationToken cancellationToken)
    {
        if (id is null)
            throw new ArgumentNullException(nameof(id));

        using (await asyncLock.LockAsync(cancellationToken))
        {
            var dbTemplate = await databaseContext.EmailTemplates.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
            if (dbTemplate is null)
                return null;

            if (dbTemplate.Type is not null)
                await cache.AddOrUpdateAsync(dbTemplate.Type.Value, dbTemplate, cancellationToken);

            return dbTemplate;
        }
    }

    public async Task<EmailTemplate?> TryGetTemplateAsync(EmailTemplateType type, CancellationToken cancellationToken)
    {
        using (await asyncLock.LockAsync(cancellationToken))
        {
            var value = await cache.TryGetAsync(type, cancellationToken);
            if (value is not null)
                return value;

            var dbTemplate = await databaseContext.EmailTemplates.SingleOrDefaultAsync(e => e.Type == type, cancellationToken);
            if (dbTemplate == null)
                return null;

            await cache.AddOrUpdateAsync(dbTemplate.Type!.Value, dbTemplate, cancellationToken);

            return dbTemplate;
        }
    }

    public async Task CreateEmailTemplateAsync(EmailTemplate template, CancellationToken cancellationToken)
    {
        if (template is null)
            throw new ArgumentNullException(nameof(template));

        using (await asyncLock.LockAsync(cancellationToken))
        {
            databaseContext.EmailTemplates.Add(template);
            await databaseContext.SaveChangesAsync(cancellationToken);

            if (template.Type is not null)
                await cache.AddOrUpdateAsync(template.Type.Value, template, cancellationToken);
        }
    }

    public async Task UpdateEmailTemplateAsync(EmailTemplate template, EmailTemplateType? oldType, CancellationToken cancellationToken)
    {
        if (template is null)
            throw new ArgumentNullException(nameof(template));

        using (await asyncLock.LockAsync(cancellationToken))
        {
            if (oldType is not null)
                await cache.RemoveAsync(oldType.Value, cancellationToken);

            databaseContext.EmailTemplates.Update(template);
            await databaseContext.SaveChangesAsync(cancellationToken);

            if (template.Type is not null)
                await cache.AddOrUpdateAsync(template.Type.Value, template, cancellationToken);
        }
    }

    public async Task<bool> RemoveEmailTemplateAsync(string id, CancellationToken cancellationToken)
    {
        if (id is null)
            throw new ArgumentNullException(nameof(id));

        using (await asyncLock.LockAsync(cancellationToken))
        {
            var template = await databaseContext.EmailTemplates.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
            if (template is null)
                return false;

            if (template.Type is not null)
                await cache.RemoveAsync(template.Type.Value, cancellationToken);

            databaseContext.EmailTemplates.Remove(template);
            await databaseContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
