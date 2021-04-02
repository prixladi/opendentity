using System;
using Microsoft.AspNetCore.Mvc;
using Shamyr.AspNetCore.HttpErrors;

namespace Shamyr.AspNetCore.Handlers.Exceptions
{
    public abstract class CodeExceptionHadlerBase<TException>: ExceptionHandlerBase<TException>
        where TException : Exception
    {
        protected abstract int StatusCode { get; }

        protected override ActionResult DoHandle(TException ex)
        {
            return ex.ToHttpResponseModel(StatusCode);
        }
    }
}
