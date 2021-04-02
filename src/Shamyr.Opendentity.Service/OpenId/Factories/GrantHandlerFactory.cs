using System;
using System.Linq;
using OpenIddict.Abstractions;
using Shamyr.Factories;
using Shamyr.Opendentity.Service.OpenId.GrantValidators;

namespace Shamyr.Opendentity.Service.OpenId.Factories
{
    public class GrantHandlerFactory: FactoryBase<IGrantHandler>, IGrantHandlerFactory
    {
        public GrantHandlerFactory(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        public IGrantHandler? Create(OpenIddictRequest request)
        {
            return GetComponents().SingleOrDefault(x => x.CanHandle(request));
        }
    }
}
