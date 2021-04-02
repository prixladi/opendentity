using System;
using Shamyr.AspNetCore.Handlers.Exceptions;

namespace Shamyr.AspNetCore.Factories
{
    public interface IExceptionHandlerFactory
    {
        IExceptionHandler? TryCreate(Exception ex);
    }
}