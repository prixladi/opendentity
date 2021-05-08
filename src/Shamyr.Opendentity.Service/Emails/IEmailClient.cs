using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Shamyr.Opendentity.Service.Emails
{
    public interface IEmailClient
  {
    Task SendEmailAsync(MailAddress recipient, string subject, EmailBody body, CancellationToken cancellationToken);
  }
}