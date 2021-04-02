﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.Service.OpenId.Exceptions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Shamyr.Opendentity.Service.OpenId.GrantValidators
{
    public class PasswordGrantHandler: IGrantHandler
    {
        private readonly SignInManager<ApplicationUser> fSignInManager;
        private readonly UserManager<ApplicationUser> fUserManager;

        public PasswordGrantHandler(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            fSignInManager = signInManager;
            fUserManager = userManager;
        }

        public bool CanHandle(OpenIddictRequest request)
        {
            return request.IsPasswordGrantType();
        }

        public async Task<ClaimsPrincipal> HandleAsync(OpenIddictRequest request, CancellationToken cancellationToken)
        {
            var user = await fUserManager.FindByNameAsync(request.Username);
            if (user == null)
                throw Forbidden();

            var result = await fSignInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
                throw Forbidden();

            var principal = await fSignInManager.CreateUserPrincipalAsync(user);
            principal.SetScopes(request.GetScopes());

            foreach (var claim in principal.Claims)
                claim.SetDestinations(Utils.GetClaimDestinations(claim));

            return principal;
        }

        private static ForbiddenException Forbidden()
        {
            var properties = new AuthenticationProperties(new Dictionary<string, string?>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The username or password is invalid."
            });

            return new ForbiddenException(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, properties);
        }
    }
}