using System.Linq;
using Microsoft.AspNetCore.Http;
using Opendentity.OpenId.Extensions;
using Shamyr.AspNetCore.Handlers.Exceptions;
using Shamyr.AspNetCore.HttpErrors;

namespace Opendentity.Service.Handlers.Exceptions;

public class IdentityExceptionHandler: CodeExceptionHadlerBase<IdentityException>
{
    protected override int StatusCode => StatusCodes.Status400BadRequest;

    protected override HttpErrorResponseModel CreateModel(IdentityException ex)
    {
        return new HttpErrorResponseModel
        {
            Message = ex.Result.ToString(),
            Errors = ex.Result.Errors.Select(x => new ErrorModel
            {
                Name = x.Code,
                Code = x.Code,
                Message = x.Description
            }).ToArray()
        };
    }
}
