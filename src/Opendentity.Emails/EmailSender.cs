using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Opendentity.Emails.Clients;

namespace Opendentity.Emails;

internal class EmailSender: IEmailSender
{
    private readonly IEmailClientFactory emailClientFactory;
    private readonly ILogger<EmailSender> logger;

    public EmailSender(IEmailClientFactory emailClientFactory, ILogger<EmailSender> logger)
    {
        this.emailClientFactory = emailClientFactory;
        this.logger = logger;
    }

    public async Task SendEmailAsync(string[] recipients, EmailDataDto dto, CancellationToken cancellationToken)
    {
        await SendEmailAsync(recipients, dto.Subject, dto.Body, cancellationToken);
    }

    public async Task SendEmailAsync(string recipient, EmailDataDto dto, CancellationToken cancellationToken)
    {
        await SendEmailAsync(new[] { recipient }, dto.Subject, dto.Body, cancellationToken);
    }

    private async Task SendEmailAsync(string[] recipients, string subject, EmailBodyDto body, CancellationToken cancellationToken)
    {
        if (recipients is null)
            throw new ArgumentNullException(nameof(recipients));
        if (subject is null)
            throw new ArgumentNullException(nameof(subject));
        if (body is null)
            throw new ArgumentNullException(nameof(body));

        var client = emailClientFactory.Create();
        try
        {
            await client.SendEmailAsync(recipients, subject, body, cancellationToken);
            logger.LogInformation("Email with subject '{subject}' to '{addresses}' succesfuly sent.", subject, string.Join(", ", recipients));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured while sending with subject '{subject}' to '{addresses}'.", subject, string.Join(", ", recipients));
        }
    }
}
