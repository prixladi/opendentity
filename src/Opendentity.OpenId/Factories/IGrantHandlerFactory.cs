using OpenIddict.Abstractions;
using Opendentity.OpenId.Handlers;

namespace Opendentity.OpenId.Factories;

public interface IGrantHandlerFactory
{
    IGrantHandler? Create(OpenIddictRequest request);
}
