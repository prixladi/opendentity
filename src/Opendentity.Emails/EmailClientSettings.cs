using System;
using System.Collections.Generic;
using MailKit.Security;

namespace Opendentity.Emails;

public record EmailClientSettings
{
    public EmailClientType ClientType { get; set; }
    public ApiEmailSettings? Api { get; set; }
    public SmtpEmailSettings? Smtp { get; set; }
}

public record ApiEmailSettings
{
    public Uri ServerUrl { get; init; } = default!;
    public string Sender { get; init; } = default!;
    public string? SenderDisplayName { get; init; }
    public KeyValuePair<string, string>[] Headers { get; init; } = Array.Empty<KeyValuePair<string, string>>();
}

public record SmtpEmailSettings
{
    public string Host { get; init; } = default!;
    public int Port { get; init; }
    public SecureSocketOptions UseSsl { get; init; }

    public string? Username { get; init; }
    public string? Password { get; init; }

    public string Sender { get; init; } = default!;
    public string? SenderDisplayName { get; init; }

    public bool EnableProtocolLogging { get; init; }
}
