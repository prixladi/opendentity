using System.Net.Mail;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Opendentity.Database.Entities;
using Opendentity.Domain.Extensions;
using Opendentity.Domain.Models;
using Opendentity.Domain.Services;
using Opendentity.Domain.Settings;
using Opendentity.Emails;
using Opendentity.OpenId;
using Opendentity.OpenId.Extensions;
using Opendentity.OpenId.Services;
using Shamyr.Exceptions;

namespace Opendentity.Domain.CQRS;

public record CreateUserCommand(CreateUserModel Model): IRequest<CreatedModel>;

public class CreateUserCommandHandler: IRequestHandler<CreateUserCommand, CreatedModel>
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IEmailTemplateManager emailTemplateManager;
    private readonly IOptions<OpenIdSettings> options;
    private readonly IOptions<UISettings> uiOptions;
    private readonly IEmailClient emailClient;
    private readonly IUserValidationService userValidationService;

    public CreateUserCommandHandler(
        UserManager<ApplicationUser> userManager,
        IEmailTemplateManager emailTemplateManager,
        IOptions<OpenIdSettings> options,
        IOptions<UISettings> uiOptions,
        IEmailClient emailClient,
        IUserValidationService userValidationService)
    {
        this.userManager = userManager;
        this.emailTemplateManager = emailTemplateManager;
        this.options = options;
        this.uiOptions = uiOptions;
        this.emailClient = emailClient;
        this.userValidationService = userValidationService;
    }

    public async Task<CreatedModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await ValidateUserOrThrowAsync(request.Model);
        var user = CreateUser(request.Model);

        var result = await userManager.CreateAsync(user, request.Model.Password);
        if (!result.Succeeded)
            throw new IdentityException(result);

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        await SendConfirmationEmailAsync(user.Email, token, cancellationToken);

        return new CreatedModel { Id = user.Id };
    }

    private async Task ValidateUserOrThrowAsync(CreateUserModel model)
    {
        userValidationService.ValidateUsernameOrThrow(model.UserName);

        if ((await userManager.FindByNameAsync(model.UserName)) is not null)
            throw new ConflictException($"User with username '{model.UserName}' already exist.");

        if ((await userManager.FindByEmailAsync(model.Email)) is not null)
            throw new ConflictException($"User with email '{model.Email}' already exist.");
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
        if (options.Value.RequireConfirmedAccount && template is null)
            throw new InvalidOperationException("Server doesn't have confirmation email set and confirmed account is required for user to log in.");
        else if (template is not null)
        {
            var recipient = new MailAddress(email);
            var dto = template.ToConfirmationEmail(token: token, email: email, portalUrl: uiOptions.Value.ToString());
            await emailClient.SendEmailAsync(recipient, dto, cancellationToken);
        }
    }
}
