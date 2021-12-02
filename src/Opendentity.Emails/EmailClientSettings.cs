using System;

namespace Opendentity.Emails;

public class EmailClientSettings
{
    public Uri ServerUrl { get; init; } = default!;
    public string SenderAddress { get; init; } = default!;
}
