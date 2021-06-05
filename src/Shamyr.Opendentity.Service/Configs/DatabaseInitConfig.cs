using Shamyr.Opendentity.Service.DatabaseInit;

namespace Shamyr.Opendentity.Service.Configs
{
    public record DatabaseInitConfig: IDatabaseInitConfig
    {
        public string? InitFilePath { get; } = EnvVariable.TryGet(EnvVariables._InitFilePath);
    }
}
