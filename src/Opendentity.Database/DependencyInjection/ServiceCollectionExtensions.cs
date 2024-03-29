﻿using Microsoft.Extensions.DependencyInjection;

namespace Opendentity.Database.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static DatabaseBuilder AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>();

        return new DatabaseBuilder(services);
    }
}
