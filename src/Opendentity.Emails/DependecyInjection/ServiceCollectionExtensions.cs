using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace Opendentity.Emails.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static EmailClientBuilder AddEmailClient(this IServiceCollection services)
    {
        services.AddHttpClient<IEmailClient, ApiEmailClient>()
            .AddPolicyHandler(GetRetryPolicy);

        return new EmailClientBuilder(services);
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IServiceProvider provider, HttpRequestMessage httpRequestMessage)
    {
        var logger = provider.GetRequiredService<ILogger<HttpRequestMessage>>();
        return Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .OrResult(HttpStatusCodePredicate)
                .WaitAndRetryAsync(3, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)));
    }

    private static bool HttpStatusCodePredicate(HttpResponseMessage response)
    {
        return response.StatusCode == HttpStatusCode.RequestTimeout ||
            response.StatusCode == HttpStatusCode.BadGateway ||
            response.StatusCode == HttpStatusCode.ServiceUnavailable;
    }
}
