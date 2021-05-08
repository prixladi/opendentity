using Shamyr.Opendentity.Service.DatabaseInit;

namespace Shamyr.Opendentity.Service.Configs
{
    public class DatabaseInitConfig: IDatabaseInitConfig
    {
        public string? InitFilePath => EnvVariable.TryGet(EnvVariables._InitFilePath);
    }
}
