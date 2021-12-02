using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Opendentity.Database.Entities;
using Opendentity.Domain.CQRS.Base;
using Opendentity.Domain.Extensions;
using Opendentity.Domain.Services;
using Opendentity.Domain.Settings;
using Opendentity.Emails;
using Shamyr.Exceptions;

namespace Opendentity.Domain.CQRS.Emails;

public record SendEmailConfirmationCommand(string Email): IRequest;

public class SendEmailConfirmationCommandHandler: EmailRequestHandlerBase, IRequestHandler<SendEmailConfirmationCommand>
{
    private readonly IEmailTemplateManager emailTemplateManager;
    private readonly IEmailSender emailSender;
    private readonly IOptions<UISettings> options;

    public SendEmailConfirmationCommandHandler(
        IEmailTemplateManager emailTemplateManager,
        UserManager<ApplicationUser> userManager,
        IEmailSender emailSender,
        IOptions<UISettings> options)
        : base(userManager)
    {
        this.emailTemplateManager = emailTemplateManager;
        this.emailSender = emailSender;
        this.options = options;
    }

    public async Task<Unit> Handle(SendEmailConfirmationCommand request, CancellationToken cancellationToken)
    {
        var template = await emailTemplateManager.TryGetTemplateAsync(EmailTemplateType.ConfirmationEmail, cancellationToken);
        if (template is null)
            throw new InvalidOperationException("Server doesn't have confirmation email set.");

        var user = await GetByEmailOrThrowAsync(request.Email);
        if (user.EmailConfirmed) // TODO: Should we even send information that email is confirmed?
            throw new ConflictException($"User with email '{request.Email}' has email already confirmed.");

        string? token = await userManager.GenerateEmailConfirmationTokenAsync(user);

        var dto = template.ToConfirmationEmail(token: token, email: user.Email, portalUrl: options.Value.PortalUrl);
        await emailSender.SendEmailAsync(user.Email, dto, cancellationToken);

        return Unit.Value;
    }
}
