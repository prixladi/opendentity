using System;
using Microsoft.AspNetCore.Mvc;

namespace Shamyr.AspNetCore.Handlers.Exceptions
{
    public interface IExceptionHandler
    {
        bool CanHandle(Exception exception);
        ActionResult Handle(Exception ex);
    }
}
