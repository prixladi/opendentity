using System;
using Microsoft.Extensions.DependencyInjection;

namespace Shamyr.Extensions.DependencyInjection
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
