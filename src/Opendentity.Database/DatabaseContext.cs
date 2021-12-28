using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Opendentity.Database.Entities;

namespace Opendentity.Database;

public class DatabaseContext: IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    private readonly IOptions<DatabaseSettings> options;

    public DbSet<DbSettings> DbSettings => Set<DbSettings>();
    public DbSet<EmailTemplate> EmailTemplates => Set<EmailTemplate>();

    public DatabaseContext(IOptions<DatabaseSettings> options)
    {
        this.options = options;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(options.Value.ConnectionString);
        //optionsBuilder.UseNpgsql("Host=localhost;Database=idnetity;Username=admin;Password=password");
        optionsBuilder.UseOpenIddict<Application, Authorization, Scope, Token, string>();
        optionsBuilder.UseSnakeCaseNamingConvention();

        optionsBuilder.LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
    }
}
