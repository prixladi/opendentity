using System;
using Microsoft.Extensions.DependencyInjection;

namespace Shamyr.DependencyInjection
{
  [AttributeUsage(AttributeTargets.Class)]
  public abstract class LifetimeAttribute: Attribute
  {
    public ServiceLifetime Lifetime { get; }

    protected LifetimeAttribute(ServiceLifetime lifetime)
    {
      Lifetime = lifetime;
    }
  }
}
