using System;
using Microsoft.AspNetCore.Mvc;

namespace Shamyr.AspNetCore.HttpErrors
{
    public static class ExceptionExtensions
    {
        public static ObjectResult ToHttpResponseModel(this Exception exception, int statusCode)
        {
            if (exception is null)
                throw new ArgumentNullException(nameof(exception));

            var errorResponse = new HttpResponseModel
            {
                Message = exception.GetType().Name,
                Errors = new ErrorModel[]
                {
                    new ErrorModel
                    {
                        Name = exception.GetType().Name,
                        Message = exception.Message
                    }
                }
            };

            return new ObjectResult(errorResponse) { StatusCode = statusCode };
        }
    }

}
