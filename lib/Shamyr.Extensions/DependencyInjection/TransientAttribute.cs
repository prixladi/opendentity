using Microsoft.Extensions.DependencyInjection;

namespace Shamyr.Extensions.DependencyInjection
{
  public sealed class TransientAttribute: LifetimeAttribute
  {
    public TransientAttribute()
      : base(ServiceLifetime.Transient) { }
  }
}
