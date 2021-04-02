using System;
using System.Linq;
using Shamyr.AspNetCore.Handlers.Exceptions;
using Shamyr.Factories;

namespace Shamyr.AspNetCore.Factories
{
    public class ExceptionHandlerFactory: FactoryBase<IExceptionHandler>, IExceptionHandlerFactory
    {
        public ExceptionHandlerFactory(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        public IExceptionHandler? TryCreate(Exception ex)
        {
            return GetComponents().SingleOrDefault(x => x.CanHandle(ex));
        }
    }
}
