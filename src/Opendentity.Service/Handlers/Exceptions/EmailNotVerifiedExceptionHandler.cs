using System.Collections.Generic;
using Opendentity.OpenId.Exceptions;
using Shamyr.AspNetCore.Handlers.Exceptions;
using Shamyr.AspNetCore.HttpErrors;

namespace Opendentity.Service.Handlers.Exceptions;

public class EmailNotVerifiedExceptionHandler: CodeExceptionHadlerBase<EmailNotVerifiedException>
{
    protected override int StatusCode => Constants.CustomStatusCodes._EmailNotVerified;

    protected override HttpErrorResponseModel CreateModel(EmailNotVerifiedException ex)
    {
        return new HttpErrorResponseModel
        {
            Message = "Email is not verified",
            Features = new KeyValuePair<string, object>[]
            {
                KeyValuePair.Create<string, object>("email", ex.Email)
            }
        };
    }
}
