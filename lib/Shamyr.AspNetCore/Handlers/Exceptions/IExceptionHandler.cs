using Microsoft.AspNetCore.Mvc;
using System;

namespace Shamyr.AspNetCore.Handlers.Exceptions
{
    public interface IExceptionHandler
    {
        bool CanHandle(Exception exception);
        ActionResult Handle(Exception ex);
    }
}
