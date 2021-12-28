using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Opendentity.Emails.Clients;

internal class ApiEmailClient: IEmailClient
{
    private readonly IOptions<EmailClientSettings> options;
    private readonly HttpClient httpClient;

    public EmailClientType Type => EmailClientType.Api;

    public ApiEmailClient(IOptions<EmailClientSettings> options, HttpClient httpClient)
    {
        this.options = options;
        this.httpClient = httpClient;
    }

    public async Task SendEmailAsync(string[] recipients, string subject, EmailBodyDto body, CancellationToken cancellationToken)
    {
        var settings = options.Value.Api ?? throw new Exception("Missing api configuration for emails.");

        var sendObject = new
        {
            recipients,
            subject,
            message = body.Content,
            isBodyHtml = body.IsHtml,
            sender = settings.Sender,
            senderDisplayName = settings.SenderDisplayName
        };

        var request = new HttpRequestMessage(HttpMethod.Post, settings.ServerUrl);
        foreach (var header in settings.Headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var content = JsonContent.Create(sendObject, options: serializerOptions);
        request.Content = content;

        await httpClient.SendAsync(request, cancellationToken);
    }
}
