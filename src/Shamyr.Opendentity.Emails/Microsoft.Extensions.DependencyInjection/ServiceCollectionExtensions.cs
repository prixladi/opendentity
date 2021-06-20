using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Shamyr.Opendentity.Emails;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEmailClient(this IServiceCollection services)
        {
            services.AddHttpClient<IEmailClient, EmailClient>()
                .AddPolicyHandler(GetRetryPolicy);

            return services;
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
}
