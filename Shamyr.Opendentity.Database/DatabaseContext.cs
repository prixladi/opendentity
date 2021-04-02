using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shamyr.Opendentity.Database.Entities;

namespace Shamyr.Opendentity.Database
{
    public class DatabaseContext: IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public DatabaseContext(DbContextOptions options)
            : base(options) { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql("Host=postgres;Database=postgress;Username=admin;Password=password");
        //    optionsBuilder.UseOpenIddict<Application, Authorization, Scope, Token, string>();
        //    optionsBuilder.UseSnakeCaseNamingConvention();
        //}
    }
}
