using System;
using Microsoft.AspNetCore.Mvc;
using Shamyr.AspNetCore.Handlers.Exceptions;
using Shamyr.Opendentity.Service.OpenId.Exceptions;

namespace Shamyr.Opendentity.Service.Handlers.Exceptions
{
    public class ForbiddenExceptionHandler: ExceptionHandlerBase<ForbiddenException>
    {
        protected override ActionResult DoHandle(ForbiddenException ex)
        {
            return new ForbidResult(ex.Scheme, ex.Properties);
        }
    }
}
