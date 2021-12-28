using OpenIddict.Abstractions;

namespace Opendentity.OpenId.Extensions;

public static class OpenIddictRequestExtensions
{
    private const string _LoginByEmailParameter = "loginByEmail";

    public static bool IsEmailLogin(this OpenIddictRequest request)
    {
        return request.TryGetParameter(_LoginByEmailParameter, out var param) && (bool)param;
    }
}
