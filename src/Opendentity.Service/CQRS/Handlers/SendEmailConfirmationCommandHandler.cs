using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Opendentity.Database.Entities;
using Shamyr.Exceptions;
using Opendentity.Emails;
using Opendentity.Service.Configs;
using Opendentity.Service.CQRS.Commands;
using Opendentity.Service.CQRS.Handlers.Base;
using Opendentity.Service.Extensions;
using Opendentity.Service.Services;

namespace Opendentity.Service.CQRS.Handlers;

public class SendEmailConfirmationCommandHandler: EmailRequestHandlerBase, IRequestHandler<SendEmailConfirmationCommand>
{
    private readonly IEmailTemplateManager emailTemplateManager;
    private readonly IEmailClient emailClient;
    private readonly IOptions<UISettings> options;

    public SendEmailConfirmationCommandHandler(
        IEmailTemplateManager emailTemplateManager,
        UserManager<ApplicationUser> userManager,
        IEmailClient emailClient,
        IOptions<UISettings> options)
        : base(userManager)
    {
        this.emailTemplateManager = emailTemplateManager;
        this.emailClient = emailClient;
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

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

        var recipient = new MailAddress(user.Email);
        var dto = template.ToConfirmationEmail(token: token, email: user.Email, portalUrl: options.Value.PortalUrl.ToString());
        await emailClient.SendEmailAsync(recipient, dto, cancellationToken);

        return Unit.Value;
    }
}
