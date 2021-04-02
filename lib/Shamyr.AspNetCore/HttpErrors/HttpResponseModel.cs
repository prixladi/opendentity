using System.Collections.Generic;

namespace Shamyr.AspNetCore.HttpErrors
{
    public record HttpResponseModel
    {
        public string? Message { get; init; }
        public ICollection<ErrorModel>? Errors { get; init; }
    }

}
