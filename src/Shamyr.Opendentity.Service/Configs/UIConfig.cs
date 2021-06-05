using System;

namespace Shamyr.Opendentity.Service.Configs
{
    public static class UIConfig
    {
        public static Uri PortalUrl { get; } = EnvVariable.Get(EnvVariables._PortalUrl, x => new Uri(x));
    }
}
