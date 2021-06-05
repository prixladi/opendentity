using OpenIddict.Abstractions;
using Shamyr.Opendentity.OpenId.Handlers;

namespace Shamyr.Opendentity.OpenId.Factories
{
    public interface IGrantHandlerFactory
    {
        IGrantHandler? Create(OpenIddictRequest request);
    }
}
