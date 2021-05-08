using Microsoft.Extensions.DependencyInjection;

namespace Shamyr.Extensions.DependencyInjection
{
  public sealed class SingletonAttribute: LifetimeAttribute
  {
    public SingletonAttribute()
      : base(ServiceLifetime.Singleton) { }
  }
}
