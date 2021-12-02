using System;
using System.Threading;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Opendentity.Emails.Clients;

internal class SmtpEmailClient: IEmailClient
{
    private readonly IOptions<EmailClientSettings> options;

    public EmailClientType Type => EmailClientType.Smtp;

    public SmtpEmailClient(IOptions<EmailClientSettings> options)
    {
        this.options = options;
    }

    public async Task SendEmailAsync(string[] recipients, string subject, EmailBodyDto body, CancellationToken cancellationToken)
    {
        var settings = options.Value.Smtp ?? throw new Exception("Missing smtp configuration for emails");
        var protocolLogger = new ProtocolLogger(Console.OpenStandardOutput());
        using var client = settings.EnableProtocolLogging ? new SmtpClient(protocolLogger) : new SmtpClient();

        await client.ConnectAsync(settings.Host, settings.Port, settings.UseSsl, cancellationToken);
        if (!string.IsNullOrEmpty(settings.Username))
        {
            await client.AuthenticateAsync(settings.Username, settings.Password, cancellationToken);
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(settings.Sender, settings.SenderDisplayName ?? settings.Sender));
        foreach (string? recipient in recipients)
            message.To.Add(new MailboxAddress(recipient, recipient));

        message.Subject = subject;
        message.Body = new TextPart(body.IsHtml ? "html" : "plain")
        {
            Text = body.Content
        };

        await client.SendAsync(message, cancellationToken);
    }
}
