using Microsoft.AspNetCore.Mvc;
using System;

namespace Shamyr.AspNetCore.Handlers.Exceptions
{
    public abstract class ExceptionHandlerBase<TException> : IExceptionHandler
        where TException : Exception
    {
        public bool CanHandle(Exception exception)
        {
            return exception is TException;
        }

        public ActionResult Handle(Exception ex)
        {
            return DoHandle((TException)ex);
        }

        protected abstract ActionResult DoHandle(TException ex);
    }
}
