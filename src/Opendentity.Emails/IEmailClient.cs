using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Opendentity.Emails;

public interface IEmailClient
{
    Task SendEmailAsync(MailAddress recipient, EmailDataDto dto, CancellationToken cancellationToken);
    Task SendEmailAsync(MailAddress[] recipients, EmailDataDto dto, CancellationToken cancellationToken);
}
