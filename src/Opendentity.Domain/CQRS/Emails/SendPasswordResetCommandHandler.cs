using System.Net.Mail;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Opendentity.Database.Entities;
using Opendentity.Domain.CQRS.Base;
using Opendentity.Domain.Extensions;
using Opendentity.Domain.Services;
using Opendentity.Domain.Settings;
using Opendentity.Emails;

namespace Opendentity.Domain.CQRS;

public record SendPasswordResetCommand(string Email): IRequest;

public class SendPasswordResetCommandHandler: EmailRequestHandlerBase, IRequestHandler<SendPasswordResetCommand>
{
    private readonly IEmailClient emailClient;
    private readonly IEmailTemplateManager emailTemplateManager;
    private readonly IOptions<UISettings> options;

    public SendPasswordResetCommandHandler(
        UserManager<ApplicationUser> userManager,
        IEmailClient emailClient,
        IEmailTemplateManager emailTemplateManager,
        IOptions<UISettings> options)
        : base(userManager)
    {
        this.emailClient = emailClient;
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
        var dto = template.ToPasswordResetEmail(token: token, email: user.Email, portalUrl: options.Value.PortalUrl.ToString());

        await emailClient.SendEmailAsync(new MailAddress(user.Email), dto, cancellationToken);

        return Unit.Value;
    }
}
