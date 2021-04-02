using Scrutor;
using Shamyr.DependencyInjection;

namespace Shamyr.Extensions.DependencyInjection
{
    public static class ScrutorExtensions
    {
        public static IImplementationTypeSelector AddConventionClasses(this IImplementationTypeSelector selector)
        {
            return selector.AddClasses(x => x.WithAttribute<SingletonAttribute>())
                .AsMatchingInterface()
                .WithSingletonLifetime()

                .AddClasses(x => x.WithAttribute<ScopedAttribute>())
                .AsMatchingInterface()
                .WithScopedLifetime()

                .AddClasses(x => x.WithoutAttribute<SingletonAttribute>().WithoutAttribute<ScopedAttribute>())
                .AsMatchingInterface()
                .WithTransientLifetime();
        }
    }
}
