using Shamyr.Opendentity.Database;

namespace Shamyr.Opendentity.Service.Configs
{
    public class DatabaseConfig: IDatabaseConfig
    {
        public string ConnectionString => EnvVariable.Get(EnvVariables._DatabaseConnectionString);
    }
}
