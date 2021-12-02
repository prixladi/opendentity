using Opendentity.OpenId.Handlers;
using OpenIddict.Abstractions;

namespace Opendentity.OpenId.Factories;

public interface IGrantHandlerFactory
{
    IGrantHandler? Create(OpenIddictRequest request);
}
