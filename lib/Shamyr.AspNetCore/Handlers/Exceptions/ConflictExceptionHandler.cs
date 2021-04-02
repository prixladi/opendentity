using Microsoft.AspNetCore.Http;
using Shamyr.Exceptions;

namespace Shamyr.AspNetCore.Handlers.Exceptions
{
    public class ConflictExceptionHandler: CodeExceptionHadlerBase<ConflictException>
    {
        protected override int StatusCode => StatusCodes.Status409Conflict;
    }
}
