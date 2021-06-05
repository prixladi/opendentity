using Microsoft.Extensions.DependencyInjection;

namespace Shamyr.Extensions.DependencyInjection
{
    public sealed class ScopedAttribute: LifetimeAttribute
    {
        public ScopedAttribute()
          : base(ServiceLifetime.Scoped) { }
    }
}
