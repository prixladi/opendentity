using System;
using Shamyr.Opendentity.Emails;

namespace Shamyr.Opendentity.Service.Configs
{
    public record EmailClientConfig: IEmailClientConfig
    {
        public Uri ServerUrl { get; } = EnvVariable.Get(EnvVariables._EmailServerUrl, x => new Uri(x));
        public string SenderAddress { get; } = EnvVariable.Get(EnvVariables._EmailSenderAddress);
    }
}
