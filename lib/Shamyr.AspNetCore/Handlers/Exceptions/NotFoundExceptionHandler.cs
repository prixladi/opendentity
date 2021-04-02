using Microsoft.AspNetCore.Http;
using Shamyr.Exceptions;

namespace Shamyr.AspNetCore.Handlers.Exceptions
{
    public class NotFoundExceptionHandler: CodeExceptionHadlerBase<NotFoundException>
    {
        protected override int StatusCode => StatusCodes.Status404NotFound;
    }
}
