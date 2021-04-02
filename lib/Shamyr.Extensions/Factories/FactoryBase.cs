using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Shamyr.Factories
{
  public abstract class FactoryBase<T> where T : notnull
  {
    private readonly IServiceProvider fServiceProvider;

    protected FactoryBase(IServiceProvider serviceProvider)
    {
      fServiceProvider = serviceProvider;
    }

    protected IEnumerable<T> GetComponents()
    {
      return fServiceProvider.GetServices<T>();
    }

    protected T GetComponent()
    {
      return fServiceProvider.GetRequiredService<T>();
    }
  }
}
