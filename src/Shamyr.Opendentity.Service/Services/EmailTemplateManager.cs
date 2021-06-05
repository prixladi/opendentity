using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shamyr.Opendentity.Database;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Threading.Async;

namespace Shamyr.Opendentity.Service.Services
{
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
                    cache.AddOrUpdate(dbTemplate.Type.Value, dbTemplate);

                return dbTemplate;
            }
        }

        public async Task<EmailTemplate?> TryGetTemplateAsync(EmailTemplateType type, CancellationToken cancellationToken)
        {
            using (await asyncLock.LockAsync(cancellationToken))
            {
                if (cache.TryGet(type, out var value))
                    return value;

                var dbTemplate = await databaseContext.EmailTemplates.SingleOrDefaultAsync(e => e.Type == type, cancellationToken);
                if (dbTemplate == null)
                    return null;

                cache.AddOrUpdate(dbTemplate.Type!.Value, dbTemplate);

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
                    cache.AddOrUpdate(template.Type.Value, template);
            }
        }

        public async Task UpdateEmailTemplateAsync(EmailTemplate template, EmailTemplateType? oldType, CancellationToken cancellationToken)
        {
            if (template is null)
                throw new ArgumentNullException(nameof(template));

            using (await asyncLock.LockAsync(cancellationToken))
            {
                if (oldType is not null)
                    cache.TryRemove(oldType.Value);

                databaseContext.EmailTemplates.Update(template);
                await databaseContext.SaveChangesAsync(cancellationToken);

                if (template.Type is not null)
                    cache.AddOrUpdate(template.Type.Value, template);
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
                    cache.TryRemove(template.Type.Value);

                databaseContext.EmailTemplates.Remove(template);
                await databaseContext.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
