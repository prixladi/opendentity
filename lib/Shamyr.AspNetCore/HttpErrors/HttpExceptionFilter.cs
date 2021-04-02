using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shamyr.AspNetCore.Factories;

namespace Shamyr.AspNetCore.HttpErrors
{
    public class HttpExceptionFilter: IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            var handlerFactory = context.HttpContext.RequestServices.GetRequiredService<IExceptionHandlerFactory>();
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<HttpExceptionFilter>>();

            var handler = handlerFactory.TryCreate(context.Exception);
            if (handler == null)
            {
                logger.LogError(context.Exception, "Unhandled exception occurred!");
                context.Result = context.Exception.ToHttpResponseModel(StatusCodes.Status200OK);
            }
            else
                context.Result = handler.Handle(context.Exception);

            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
