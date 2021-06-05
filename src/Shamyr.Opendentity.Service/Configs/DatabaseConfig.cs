using Shamyr.Opendentity.Database;

namespace Shamyr.Opendentity.Service.Configs
{
    public record DatabaseConfig: IDatabaseConfig
    {
        public string ConnectionString { get; } = EnvVariable.Get(EnvVariables._DatabaseConnectionString);
    }
}
