using Microsoft.AspNetCore.Http;
using Shamyr.Exceptions;

namespace Shamyr.AspNetCore.Handlers.Exceptions
{
    public class BadRequestExceptionHandler: CodeExceptionHadlerBase<BadRequestException>
    {
        protected override int StatusCode => StatusCodes.Status400BadRequest;
    }
}
