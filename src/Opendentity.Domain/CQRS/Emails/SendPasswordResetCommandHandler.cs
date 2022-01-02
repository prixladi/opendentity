using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Opendentity.Database.Entities;
using Opendentity.Domain.CQRS.Base;
using Opendentity.Domain.Extensions;
using Opendentity.Domain.Services;
using Opendentity.Domain.Settings;
using Opendentity.Emails;

namespace Opendentity.Domain.CQRS.Emails;

public record SendPasswordResetCommand(string Email): IRequest;

public class SendPasswordResetCommandHandler: EmailRequestHandlerBase, IRequestHandler<SendPasswordResetCommand>
{
    private readonly IEmailSender emailSender;
    private readonly IEmailTemplateManager emailTemplateManager;
    private readonly IOptions<UISettings> options;

    public SendPasswordResetCommandHandler(
        UserManager<ApplicationUser> userManager,
        IEmailSender emailSender,
        IEmailTemplateManager emailTemplateManager,
        IOptions<UISettings> options)
        : base(userManager)
    {
        this.emailSender = emailSender;
        this.emailTemplateManager = emailTemplateManager;
        this.options = options;
    }

    public async Task<Unit> Handle(SendPasswordResetCommand request, CancellationToken cancellationToken)
    {
        var template = await emailTemplateManager.TryGetTemplateAsync(EmailTemplateType.PasswordResetEmail, cancellationToken);
        if (template is null)
            throw new InvalidOperationException("Server doesn't have password reset email set.");

        var user = await GetByEmailOrThrowAsync(request.Email);
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var dto = template.ToPasswordResetEmail(token: token, email: user.Email, portalUrl: options.Value.PortalUrl);

        await emailSender.SendEmailAsync(user.Email, dto, cancellationToken);

        return Unit.Value;
    }
}
