using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Opendentity.Database.Entities;
using Opendentity.OpenId.Exceptions;
using Opendentity.OpenId.Extensions;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Opendentity.OpenId.Handlers;

public class PasswordGrantHandler: IGrantHandler
{
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IOptions<IdentityOptions> options;

    public PasswordGrantHandler(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IOptions<IdentityOptions> options)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.options = options;
    }

    public bool CanHandle(OpenIddictRequest request)
    {
        return request.IsPasswordGrantType();
    }

    public async Task<ClaimsPrincipal> HandleAsync(OpenIddictRequest request, CancellationToken cancellationToken)
    {
        var user = request.IsEmailLogin() ? await userManager.FindByEmailAsync(request.Username) : await userManager.FindByNameAsync(request.Username);
        if (user == null || user.Disabled)
            throw Forbidden();

        if (options.Value.SignIn.RequireConfirmedAccount && !user.EmailConfirmed)
            throw new EmailNotVerifiedException(user.Email);

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            throw Forbidden();

        var principal = await signInManager.CreateUserPrincipalAsync(user);
        principal.SetScopes(request.GetScopes());

        foreach (var claim in principal.Claims)
            claim.SetDestinations(Utils.GetClaimDestinations(claim, principal));

        return principal;
    }

    private static ForbiddenException Forbidden()
    {
        var properties = new AuthenticationProperties(new Dictionary<string, string?>
        {
            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The username/email or password is invalid."
        });

        return new ForbiddenException(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, properties);
    }
}
