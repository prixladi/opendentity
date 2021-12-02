using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shamyr.AspNetCore.Handlers.Exceptions;
using Shamyr.AspNetCore.HttpErrors;
using Opendentity.OpenId.Extensions;

namespace Opendentity.Service.Handlers.Exceptions;

public class IdentityExceptionHandler: ExceptionHandlerBase<IdentityException>
{
    protected override ActionResult DoHandle(IdentityException ex)
    {
        var model = new HttpErrorResponseModel
        {
            Message = ex.Result.ToString(),
            Errors = ex.Result.Errors.Select(x => new ErrorModel
            {
                Name = x.Code,
                Code = x.Code,
                Message = x.Description
            }).ToArray()
        };

        return new ObjectResult(model) { StatusCode = StatusCodes.Status400BadRequest };
    }
}
