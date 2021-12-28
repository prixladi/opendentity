using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Opendentity.OpenId.Exceptions;
using Shamyr.AspNetCore.Handlers.Exceptions;

namespace Opendentity.Service.Handlers.Exceptions;

public class ForbiddenExceptionHandler: ExceptionHandlerBase<ForbiddenException>
{
    protected override async Task DoHandleAsync(HttpContext httpContext, ForbiddenException ex)
    {
        await httpContext.ForbidAsync(ex.Scheme, ex.Properties);
    }
}
