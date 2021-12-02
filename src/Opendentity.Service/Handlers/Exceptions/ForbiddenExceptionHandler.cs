using Microsoft.AspNetCore.Mvc;
using Shamyr.AspNetCore.Handlers.Exceptions;
using Opendentity.OpenId.Exceptions;

namespace Opendentity.Service.Handlers.Exceptions;

public class ForbiddenExceptionHandler: ExceptionHandlerBase<ForbiddenException>
{
    protected override ActionResult DoHandle(ForbiddenException ex)
    {
        return new ForbidResult(ex.Scheme, ex.Properties);
    }
}
