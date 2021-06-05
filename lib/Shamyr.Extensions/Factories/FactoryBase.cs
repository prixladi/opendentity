using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Shamyr.Extensions.Factories
{
    public abstract class FactoryBase<T> where T : notnull
    {
        private readonly IServiceProvider serviceProvider;

        protected FactoryBase(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        protected IEnumerable<T> GetComponents()
        {
            return serviceProvider.GetServices<T>();
        }

        protected T GetComponent()
        {
            return serviceProvider.GetRequiredService<T>();
        }
    }
}
