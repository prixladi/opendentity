using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Shamyr.Opendentity.Service.Emails
{
    // TODO: This is just testing email client using my personal public API -> Don't use this in production!
    // TODO: Implement client using some service like Mailgun.
    public class EmailClient: IEmailClient
    {
        private readonly IEmailClientConfig fConfig;
        private readonly ILogger<EmailClient> fLogger;
        private readonly HttpClient httpClient;

        public EmailClient(IEmailClientConfig config, ILogger<EmailClient> logger, HttpClient httpClient)
        {
            fConfig = config;
            fLogger = logger;
            this.httpClient = httpClient;
        }

        public async Task SendEmailAsync(MailAddress recipient, string subject, EmailBody body, CancellationToken cancellationToken)
        {
            if (recipient is null)
                throw new ArgumentNullException(nameof(recipient));
            if (subject is null)
                throw new ArgumentNullException(nameof(subject));
            if (body is null)
                throw new ArgumentNullException(nameof(body));

            using (fLogger.BeginScope(new object()))
            {
                try
                {
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string?, string?>("to", recipient.Address),
                        new KeyValuePair<string?, string?>("subject", subject),
                        new KeyValuePair<string?, string?>("message", body.Content),
                        new KeyValuePair<string?, string?>("isBodyHtml", body.IsHtml.ToString().ToLower()),
                        new KeyValuePair<string?, string?>("sender", fConfig.SenderAddress)
                    });

                    await httpClient.PostAsync(fConfig.ServerUrl, formContent, cancellationToken);
                    fLogger.LogInformation($"Email with subject '{subject}' from '{fConfig.SenderAddress}' to '{recipient.Address}' succesfuly sent.");
                }
                catch (Exception ex)
                {
                    fLogger.LogError(ex, $"Error occured while sending with subject '{subject}' from '{fConfig.SenderAddress}' to '{recipient.Address}'.");
                }
            }
        }
    }
}
