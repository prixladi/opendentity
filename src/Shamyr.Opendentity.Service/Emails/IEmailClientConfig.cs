using System;

namespace Shamyr.Opendentity.Service.Emails
{
    public interface IEmailClientConfig
    {
        public Uri ServerUrl { get; }
        public string SenderAddress { get; }
    }
}
