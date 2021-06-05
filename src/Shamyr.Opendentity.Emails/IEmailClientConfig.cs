using System;

namespace Shamyr.Opendentity.Emails
{
    public interface IEmailClientConfig
    {
        public Uri ServerUrl { get; }
        public string SenderAddress { get; }
    }
}
