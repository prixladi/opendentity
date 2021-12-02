using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Opendentity.Database.Entities;
using Opendentity.OpenId.Exceptions;
using Opendentity.OpenId.Extensions;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Opendentity.OpenId.Handlers;

public class GoogleGrandHandler: IGrantHandler
{
    private const string _IdTokenParameter = "id_token";

    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly UserManager<ApplicationUser> userManager;

    public GoogleGrandHandler(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
    }

    public bool CanHandle(OpenIddictRequest request)
    {
        return request.GrantType == CustomGrants._Google;
    }

    public async Task<ClaimsPrincipal> HandleAsync(OpenIddictRequest request, CancellationToken cancellationToken)
    {
        if (!request.TryGetParameter(_IdTokenParameter, out var parameter))
            throw Forbidden("Id token is required for google grant.");

        GoogleJsonWebSignature.Payload payload;
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync((string)parameter!);
        }
        catch (InvalidJwtException)
        {
            throw Forbidden("Google id token is invalid or expired.");
        }

        var user = await userManager.FindByEmailAsync(payload.Email);
        if (user is null)
        {
            user = payload.ToUser();
            var result = await userManager.CreateAsync(user);
            if (!result.Succeeded)
                throw new IdentityException(result);
        }
        else if (!user.EmailConfirmed)
        {
            // By logging with google we automaticaly validate user's email, hence confirmed
            user.EmailConfirmed = true;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new IdentityException(result);
        }

        var principal = await signInManager.CreateUserPrincipalAsync(user);
        principal.SetScopes(request.GetScopes());

        foreach (var claim in principal.Claims)
            claim.SetDestinations(Utils.GetClaimDestinations(claim, principal));

        return principal;
    }

    private static ForbiddenException Forbidden(string description)
    {
        var properties = new AuthenticationProperties(new Dictionary<string, string?>
        {
            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = description
        });

        return new ForbiddenException(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, properties);
    }
}
