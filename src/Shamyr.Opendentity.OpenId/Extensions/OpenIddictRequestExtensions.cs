using OpenIddict.Abstractions;

namespace Shamyr.Opendentity.OpenId.Extensions
{
    public static class OpenIddictRequestExtensions
    {
        private const string _LogByEmailParameter = "logByEmail";

        public static bool IsEmailLogin(this OpenIddictRequest request)
        {
            return request.TryGetParameter(_LogByEmailParameter, out var param) && (bool)param;
        }
    }
}
