using System.Collections.Generic;

namespace Shamyr.AspNetCore.HttpErrors
{
    public record ErrorModel
    {
        public string Name { get; init; } = default!;
        public string Message { get; init; } = default!;
        public string? Property { get; init; }
        public IDictionary<string, object>? Extensions { get; init; }
    }
}
