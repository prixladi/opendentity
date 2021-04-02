using Microsoft.Extensions.DependencyInjection;

namespace Shamyr.DependencyInjection
{
  public sealed class TransientAttribute: LifetimeAttribute
  {
    public TransientAttribute()
      : base(ServiceLifetime.Transient) { }
  }
}
