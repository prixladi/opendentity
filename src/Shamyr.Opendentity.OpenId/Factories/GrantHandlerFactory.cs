﻿using System;
using System.Linq;
using OpenIddict.Abstractions;
using Shamyr.Extensions.Factories;
using Shamyr.Opendentity.OpenId.Handlers;

namespace Shamyr.Opendentity.OpenId.Factories
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