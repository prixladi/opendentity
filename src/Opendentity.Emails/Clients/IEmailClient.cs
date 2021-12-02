using System.Threading;
using System.Threading.Tasks;

namespace Opendentity.Emails.Clients;

internal interface IEmailClient
{
    EmailClientType Type { get; }
    Task SendEmailAsync(string[] recipients, string subject, EmailBodyDto body, CancellationToken cancellationToken);
}
