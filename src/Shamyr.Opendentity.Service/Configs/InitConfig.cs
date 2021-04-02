namespace Shamyr.Opendentity.Service.Configs
{
    public static class InitConfig
    {
        public static string AdminName => EnvVariable.TryGet(EnvVariables._InitialAdminName, "admin");
        public static string AdminPassword => EnvVariable.TryGet(EnvVariables._InitialAdminPassword, "a96B2*x$54aAA");
    }
}
