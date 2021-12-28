using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Opendentity.Database.Entities;
using Opendentity.Domain.Extensions;
using Opendentity.Domain.Models;
using Opendentity.Domain.Services;
using Opendentity.Domain.Settings;
using Opendentity.Emails;
using Opendentity.OpenId.Extensions;
using Shamyr.Exceptions;

namespace Opendentity.Domain.CQRS.Users;

public record CreateUserCommand(CreateUserModel Model): IRequest<CreatedModel>;

public class CreateUserCommandHandler: IRequestHandler<CreateUserCommand, CreatedModel>
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IEmailTemplateManager emailTemplateManager;
    private readonly IOptions<IdentityOptions> options;
    private readonly IOptions<UISettings> uiOptions;
    private readonly IEmailSender emailSender;

    public CreateUserCommandHandler(
        UserManager<ApplicationUser> userManager,
        IEmailTemplateManager emailTemplateManager,
        IOptions<IdentityOptions> options,
        IOptions<UISettings> uiOptions,
        IEmailSender emailSender)
    {
        this.userManager = userManager;
        this.emailTemplateManager = emailTemplateManager;
        this.options = options;
        this.uiOptions = uiOptions;
        this.emailSender = emailSender;
    }

    public async Task<CreatedModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await ValidateUserOrThrowAsync(request.Model);
        var user = CreateUser(request.Model);

        var result = await userManager.CreateAsync(user, request.Model.Password);
        if (!result.Succeeded)
            throw new IdentityException(result);

        string? token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        await SendConfirmationEmailAsync(user.Email, token, cancellationToken);

        return new CreatedModel { Id = user.Id };
    }

    private async Task ValidateUserOrThrowAsync(CreateUserModel model)
    {
        if (await userManager.FindByNameAsync(model.UserName) is not null)
            throw new ConflictException($"User with username '{model.UserName}' already exist.")
            {
                ErrorCode = DomainConstants.ErrorCodes._UsernameExists
            };

        if (await userManager.FindByEmailAsync(model.Email) is not null)
            throw new ConflictException($"User with email '{model.Email}' already exist.")
            {
                ErrorCode = DomainConstants.ErrorCodes._EmailExists
            };
    }

    private ApplicationUser CreateUser(CreateUserModel model)
    {
        return new ApplicationUser(model.UserName)
        {
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            ImageUrl = model.ImageUrl
        };
    }

    private async Task SendConfirmationEmailAsync(string email, string token, CancellationToken cancellationToken)
    {
        var template = await emailTemplateManager.TryGetTemplateAsync(EmailTemplateType.ConfirmationEmail, cancellationToken);
        if (options.Value.SignIn.RequireConfirmedAccount && template is null)
        {
            throw new InvalidOperationException("Server doesn't have confirmation email set and confirmed account is required for user to log in.");
        }
        else if (template is not null)
        {
            var dto = template.ToConfirmationEmail(token: token, email: email, portalUrl: uiOptions.Value.PortalUrl);
            await emailSender.SendEmailAsync(email, dto, cancellationToken);
        }
    }
}
