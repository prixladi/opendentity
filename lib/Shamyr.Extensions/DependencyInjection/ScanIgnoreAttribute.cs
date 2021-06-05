using System;

namespace Shamyr.Extensions.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ScanIgnoreAttribute: Attribute
    {
    }
}
