using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Opendentity.Database.Entities;

namespace Opendentity.Service.Services;

public class EmailTemplateCache: IEmailTemplateCache
{
    private readonly IDistributedCache distributedCache;

    private static DistributedCacheEntryOptions Options => new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
        SlidingExpiration = TimeSpan.FromMinutes(1)
    };

    public EmailTemplateCache(IDistributedCache distributedCache)
    {
        this.distributedCache = distributedCache;
    }

    public async Task<EmailTemplate?> TryGetAsync(EmailTemplateType type, CancellationToken cancellationToken)
    {
        var key = CreateKey(type);
        var result = await distributedCache.GetStringAsync(key, cancellationToken);
        if (result is null)
            return null;

        return JsonSerializer.Deserialize<EmailTemplate>(result)!;
    }

    public async Task AddOrUpdateAsync(EmailTemplateType type, EmailTemplate template, CancellationToken cancellationToken)
    {
        var key = CreateKey(type);
        var data = JsonSerializer.Serialize(template);

        await distributedCache.SetStringAsync(key, data, Options, cancellationToken);
    }

    public async Task RemoveAsync(EmailTemplateType type, CancellationToken cancellationToken)
    {
        var key = CreateKey(type);
        await distributedCache.RemoveAsync(key, cancellationToken);
    }

    public async Task ClearAsync(CancellationToken cancellationToken)
    {
        await distributedCache.RemoveAsync(CreateKey(EmailTemplateType.ConfirmationEmail), cancellationToken);
        await distributedCache.RemoveAsync(CreateKey(EmailTemplateType.PasswordResetEmail), cancellationToken);
    }

    private string CreateKey(EmailTemplateType type)
    {
        return $"template-by-type-{type}";
    }
}
