using Microsoft.EntityFrameworkCore;
using Shamyr.Opendentity.Database;
using Shamyr.Opendentity.Database.Entities;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DatabaseContext>(opt =>
            {
                opt.UseNpgsql(connectionString);
                opt.UseOpenIddict<Application, Authorization, Scope, Token, string>();
                opt.UseSnakeCaseNamingConvention();
            });
        }
    }
}
