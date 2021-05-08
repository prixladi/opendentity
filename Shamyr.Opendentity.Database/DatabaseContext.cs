﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shamyr.Opendentity.Database.Entities;

namespace Shamyr.Opendentity.Database
{
    public class DatabaseContext: IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private readonly IDatabaseConfig config;

        public DbSet<Settings> Settings => Set<Settings>();

        public DatabaseContext(IDatabaseConfig config)
        {
            this.config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(config.ConnectionString);
            optionsBuilder.UseOpenIddict<Application, Authorization, Scope, Token, string>();
            optionsBuilder.UseSnakeCaseNamingConvention();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        }
    }
}
