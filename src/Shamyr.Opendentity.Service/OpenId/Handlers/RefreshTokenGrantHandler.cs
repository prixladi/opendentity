using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.Service.OpenId.Exceptions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Shamyr.Opendentity.Service.OpenId.GrantValidators
{
    public class RefreshTokenGrantHandler: IGrantHandler
    {
        private readonly SignInManager<ApplicationUser> fSignInManager;
        private readonly UserManager<ApplicationUser> fUserManager;
        private readonly IHttpContextAccessor fHttpContextAccessor;

        public RefreshTokenGrantHandler(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            fSignInManager = signInManager;
            fUserManager = userManager;
            fHttpContextAccessor = httpContextAccessor;
        }

        public bool CanHandle(OpenIddictRequest request)
        {
            return request.IsRefreshTokenGrantType();
        }

        public async Task<ClaimsPrincipal> HandleAsync(OpenIddictRequest request, CancellationToken cancellationToken)
        {
            if (fHttpContextAccessor.HttpContext == null)
                throw new InvalidOperationException("Unable to retrieve HttpContext from HttpContextAccessor.");

            var result = await fHttpContextAccessor.HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            var user = await fUserManager.GetUserAsync(result.Principal);
            if (user == null)
                throw Forbidden("The refresh token is no longer valid.");

            if (!await fSignInManager.CanSignInAsync(user))
                throw Forbidden("The user is no longer allowed to sign in.");

            var principal = await fSignInManager.CreateUserPrincipalAsync(user);

            foreach (var claim in principal.Claims)
                claim.SetDestinations(Utils.GetClaimDestinations(claim));

            return principal;
        }

        private static ForbiddenException Forbidden(string message)
        {
            var properties = new AuthenticationProperties(new Dictionary<string, string?>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = message
            });

            return new ForbiddenException(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, properties);
        }
    }
}