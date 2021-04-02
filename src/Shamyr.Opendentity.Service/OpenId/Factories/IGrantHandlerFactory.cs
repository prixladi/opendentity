using OpenIddict.Abstractions;
using Shamyr.Opendentity.Service.OpenId.GrantValidators;

namespace Shamyr.Opendentity.Service.OpenId.Factories
{
    public interface IGrantHandlerFactory
    {
        IGrantHandler? Create(OpenIddictRequest request);
    }
}
