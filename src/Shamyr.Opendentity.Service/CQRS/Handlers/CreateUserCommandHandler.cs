using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shamyr.Exceptions;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.Emails;
using Shamyr.Opendentity.OpenId;
using Shamyr.Opendentity.OpenId.Services;
using Shamyr.Opendentity.Service.CQRS.Commands;
using Shamyr.Opendentity.Service.Extensions;
using Shamyr.Opendentity.Service.Models;
using Shamyr.Opendentity.Service.Services;

namespace Shamyr.Opendentity.Service.CQRS.Handlers
{
    public class CreateUserCommandHandler: IRequestHandler<CreateUserCommand, CreatedModel>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailTemplateManager emailTemplateManager;
        private readonly IOpenIdConfig openIdConfig;
        private readonly IEmailClient emailClient;
        private readonly IUserValidationService userValidationService;

        public CreateUserCommandHandler(
            UserManager<ApplicationUser> userManager,
            IEmailTemplateManager emailTemplateManager,
            IOpenIdConfig openIdConfig,
            IEmailClient emailClient,
            IUserValidationService userValidationService)
        {
            this.userManager = userManager;
            this.emailTemplateManager = emailTemplateManager;
            this.openIdConfig = openIdConfig;
            this.emailClient = emailClient;
            this.userValidationService = userValidationService;
        }

        public async Task<CreatedModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await ValidateUserOrThrowAsync(request.Model);
            var user = CreateUser(request.Model);

            var result = await userManager.CreateAsync(user, request.Model.Password);
            if (!result.Succeeded)
                throw new BadRequestException(result.ToString());

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
            if (openIdConfig.RequireConfirmedAccount && template is null)
                throw new InvalidOperationException("Server doesn't have confirmation email set and confirmed account is required for user to log in.");
            else if (template is not null)
            {
                var recipient = new MailAddress(email);
                var dto = template.ToConfirmationEmail(token: token, email: email);
                await emailClient.SendEmailAsync(recipient, dto, cancellationToken);
            }
        }
    }
}
