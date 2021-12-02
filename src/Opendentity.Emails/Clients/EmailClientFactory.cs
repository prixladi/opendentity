using System;
using System.Linq;
using Microsoft.Extensions.Options;
using Shamyr.Extensions.Factories;

namespace Opendentity.Emails.Clients;

internal class EmailClientFactory: FactoryBase<IEmailClient>, IEmailClientFactory
{
    private readonly IOptions<EmailClientSettings> options;

    public EmailClientFactory(IServiceProvider serviceProvider, IOptions<EmailClientSettings> options)
        : base(serviceProvider)
    {
        this.options = options;
    }

    public IEmailClient Create()
    {
        return GetComponents()
            .SingleOrDefault(x => x.Type == options.Value.ClientType)
            ?? throw new InvalidOperationException($"Unable to find client of type '{options.Value.ClientType}'.");
    }
}
