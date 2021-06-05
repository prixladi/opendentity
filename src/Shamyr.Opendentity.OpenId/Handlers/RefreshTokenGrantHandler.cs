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
using Shamyr.Opendentity.OpenId.Exceptions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Shamyr.Opendentity.OpenId.Handlers
{
    public class RefreshTokenGrantHandler: IGrantHandler
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IOpenIdConfig config;

        public RefreshTokenGrantHandler(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IOpenIdConfig config)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            this.config = config;
        }

        public bool CanHandle(OpenIddictRequest request)
        {
            return request.IsRefreshTokenGrantType();
        }

        public async Task<ClaimsPrincipal> HandleAsync(OpenIddictRequest request, CancellationToken cancellationToken)
        {
            if (httpContextAccessor.HttpContext == null)
                throw new InvalidOperationException("Unable to retrieve HttpContext from HttpContextAccessor.");

            var result = await httpContextAccessor.HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            var user = await userManager.GetUserAsync(result.Principal);
            if (user == null || user.Disabled)
                throw Forbidden("The refresh token is no longer valid.");

            if (config.RequireConfirmedAccount && !user.EmailConfirmed)
                throw new EmailNotVerifiedException(user.Email);

            if (!await signInManager.CanSignInAsync(user))
                throw Forbidden("The user is no longer allowed to sign in.");

            var principal = await signInManager.CreateUserPrincipalAsync(user);
            principal.SetScopes(request.GetScopes());

            foreach (var claim in principal.Claims)
                claim.SetDestinations(Utils.GetClaimDestinations(claim, principal));

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