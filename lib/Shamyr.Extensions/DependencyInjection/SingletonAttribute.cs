using Microsoft.Extensions.DependencyInjection;

namespace Shamyr.DependencyInjection
{
  public sealed class SingletonAttribute: LifetimeAttribute
  {
    public SingletonAttribute()
      : base(ServiceLifetime.Singleton) { }
  }
}
