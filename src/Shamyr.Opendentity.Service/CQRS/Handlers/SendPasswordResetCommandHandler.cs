﻿using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.Emails;
using Shamyr.Opendentity.Service.Configs;
using Shamyr.Opendentity.Service.CQRS.Commands;
using Shamyr.Opendentity.Service.CQRS.Handlers.Base;
using Shamyr.Opendentity.Service.Extensions;
using Shamyr.Opendentity.Service.Services;

namespace Shamyr.Opendentity.Service.CQRS.Handlers
{
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
}
