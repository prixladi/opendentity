using System.Collections.Generic;

namespace Shamyr.AspNetCore.HttpErrors
{
    public record HttpErrorResponseModel
    {
        public string? Message { get; init; }
        public ICollection<ErrorModel>? Errors { get; init; }
        public (string, object)[]? Features { get; init; }
    }
}
