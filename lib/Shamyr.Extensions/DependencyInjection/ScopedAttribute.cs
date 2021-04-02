using Microsoft.Extensions.DependencyInjection;

namespace Shamyr.DependencyInjection
{
  public sealed class ScopedAttribute: LifetimeAttribute
  {
    public ScopedAttribute()
      : base(ServiceLifetime.Scoped) { }
  }
}
