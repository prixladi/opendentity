﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Opendentity.OpenId.Exceptions;
using Shamyr.AspNetCore.Handlers.Exceptions;
using Shamyr.AspNetCore.HttpErrors;

namespace Opendentity.Service.Handlers.Exceptions;

public class EmailNotVerifiedExceptionHandler: ExceptionHandlerBase<EmailNotVerifiedException>
{
    protected override ActionResult DoHandle(EmailNotVerifiedException ex)
    {
        var model = new HttpErrorResponseModel
        {
            Message = "Email is not verified",
            Features = new KeyValuePair<string, object>[]
            {
                    KeyValuePair.Create<string, object>("email", ex.Email)
            }
        };

        return new ObjectResult(model) { StatusCode = Constants.CustomStatusCodes._EmailNotVerified };
    }
}