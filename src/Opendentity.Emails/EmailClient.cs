using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Opendentity.Emails;

// TODO: This is just testing email client using my personal public API -> Don't use this in production!
// TODO: Implement client using some service like Mailgun.
public class EmailClient: IEmailClient
{
    private readonly IOptions<EmailClientSettings> options;
    private readonly ILogger<EmailClient> logger;
    private readonly HttpClient httpClient;

    public EmailClient(IOptions<EmailClientSettings> options, ILogger<EmailClient> logger, HttpClient httpClient)
    {
        this.options = options;
        this.logger = logger;
        this.httpClient = httpClient;
    }

    public async Task SendEmailAsync(MailAddress recipient, string subject, EmailBodyDto body, CancellationToken cancellationToken)
    {
        if (recipient is null)
            throw new ArgumentNullException(nameof(recipient));
        if (subject is null)
            throw new ArgumentNullException(nameof(subject));
        if (body is null)
            throw new ArgumentNullException(nameof(body));

        try
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                     new KeyValuePair<string?, string?>("to", recipient.Address),
                     new KeyValuePair<string?, string?>("subject", subject),
                     new KeyValuePair<string?, string?>("message", body.Content),
                     new KeyValuePair<string?, string?>("isBodyHtml", body.IsHtml.ToString().ToLower()),
                     new KeyValuePair<string?, string?>("sender", options.Value.SenderAddress)
                });

            await httpClient.PostAsync(options.Value.ServerUrl, formContent, cancellationToken);
            logger.LogInformation("Email with subject '{subject}' from '{senderAddress}' to '{address}' succesfuly sent.",
                subject, options.Value.SenderAddress, recipient.Address);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured while sending with subject '{subject}' from '{senderAddress}' to '{address}'.",
                subject, options.Value.SenderAddress, recipient.Address);
        }
    }

    public async Task SendEmailAsync(MailAddress recipient, EmailDataDto dto, CancellationToken cancellationToken)
    {
        if (recipient is null)
            throw new ArgumentNullException(nameof(recipient));
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        await SendEmailAsync(recipient, dto.Subject, dto.Body, cancellationToken);
    }
}
