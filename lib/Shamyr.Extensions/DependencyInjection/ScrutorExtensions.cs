using Scrutor;

namespace Shamyr.Extensions.DependencyInjection
{
    public static class ScrutorExtensions
    {
        public static IImplementationTypeSelector AddConventionClasses(this IImplementationTypeSelector selector)
        {
            return selector.AddClasses(x => x.WithAttribute<SingletonAttribute>().WithoutAttribute<ScanIgnoreAttribute>())
                .AsMatchingInterface()
                .WithSingletonLifetime()

                .AddClasses(x => x.WithAttribute<ScopedAttribute>().WithoutAttribute<ScanIgnoreAttribute>())
                .AsMatchingInterface()
                .WithScopedLifetime()

                .AddClasses(x => x.WithoutAttribute<SingletonAttribute>().WithoutAttribute<ScopedAttribute>().WithoutAttribute<ScanIgnoreAttribute>())
                .AsMatchingInterface()
                .WithTransientLifetime();
        }
    }
}
