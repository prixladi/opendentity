using System.Threading;
using System.Threading.Tasks;

namespace Opendentity.Emails;

public interface IEmailSender
{
    Task SendEmailAsync(string recipient, EmailDataDto dto, CancellationToken cancellationToken);
    Task SendEmailAsync(string[] recipients, EmailDataDto dto, CancellationToken cancellationToken);
}