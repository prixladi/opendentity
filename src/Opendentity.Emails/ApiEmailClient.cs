using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Opendentity.Emails;

public class ApiEmailClient: IEmailClient
{
    private readonly IOptions<EmailClientSettings> options;
    private readonly ILogger<ApiEmailClient> logger;
    private readonly HttpClient httpClient;

    public ApiEmailClient(IOptions<EmailClientSettings> options, ILogger<ApiEmailClient> logger, HttpClient httpClient)
    {
        this.options = options;
        this.logger = logger;
        this.httpClient = httpClient;
    }

    public async Task SendEmailAsync(MailAddress[] recipients, EmailDataDto dto, CancellationToken cancellationToken)
    {
        await SendEmailAsync(recipients, dto.Subject, dto.Body, cancellationToken);
    }

    public async Task SendEmailAsync(MailAddress recipient, EmailDataDto dto, CancellationToken cancellationToken)
    {
        await SendEmailAsync(new[] { recipient }, dto.Subject, dto.Body, cancellationToken);
    }

    private async Task SendEmailAsync(MailAddress[] recipients, string subject, EmailBodyDto body, CancellationToken cancellationToken)
    {
        if (recipients is null)
            throw new ArgumentNullException(nameof(recipients));
        if (subject is null)
            throw new ArgumentNullException(nameof(subject));
        if (body is null)
            throw new ArgumentNullException(nameof(body));

        var addresses = recipients.Select(x => x.Address);

        try
        {
            var sendObject = new
            {
                recipients = addresses,
                subject,
                message = body.Content,
                isBodyHtml = body.IsHtml,
                sender = options.Value.SenderAddress
            };


            await httpClient.PostAsJsonAsync(options.Value.ServerUrl, sendObject, cancellationToken);
            logger.LogInformation("Email with subject '{subject}' to '{addresses}' succesfuly sent.", subject, string.Join(", ", addresses));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured while sending with subject '{subject}' to '{addresses}'.", subject, string.Join(", ", addresses));
        }
    }
}
