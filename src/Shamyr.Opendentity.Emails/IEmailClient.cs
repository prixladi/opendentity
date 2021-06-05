using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Shamyr.Opendentity.Emails
{
    public interface IEmailClient
    {
        Task SendEmailAsync(MailAddress recipient, string subject, EmailBodyDto body, CancellationToken cancellationToken);
        Task SendEmailAsync(MailAddress recipient, EmailDataDto dto, CancellationToken cancellationToken);
    }
}