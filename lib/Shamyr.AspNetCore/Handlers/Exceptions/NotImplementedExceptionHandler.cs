using System;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Shamyr.AspNetCore.Handlers.Exceptions
{
    public class NotImplementedExceptionHandler: CodeExceptionHadlerBase<NotImplementedException>
    {
        protected override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}
