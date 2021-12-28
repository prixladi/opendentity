using MediatR;
using Microsoft.EntityFrameworkCore;
using Opendentity.Database;
using Opendentity.Database.Entities;
using Opendentity.Domain.Models;
using Opendentity.Domain.Services;
using Shamyr.Exceptions;

namespace Opendentity.Domain.CQRS.EmailTemplates;

public record CreateEmailTemplateCommand(CreateEmailTemplateModel Model): IRequest<CreatedModel>;

public class CreateEmailTemplateCommandHandler: IRequestHandler<CreateEmailTemplateCommand, CreatedModel>
{
    private readonly IEmailTemplateManager emailTemplateManager;
    private readonly DatabaseContext databaseContext;

    public CreateEmailTemplateCommandHandler(IEmailTemplateManager emailTemplateManager, DatabaseContext databaseContext)
    {
        this.emailTemplateManager = emailTemplateManager;
        this.databaseContext = databaseContext;
    }

    public async Task<CreatedModel> Handle(CreateEmailTemplateCommand request, CancellationToken cancellationToken)
    {
        if (await databaseContext.EmailTemplates.AnyAsync(e => e.Type == request.Model.Type, cancellationToken: cancellationToken))
            throw new ConflictException($"Email template with ID '{request.Model.Type}' already exist.");

        var template = new EmailTemplate
        {
            Type = request.Model.Type,
            Subject = request.Model.Subject,
            ContentTemplate = request.Model.ContentTemplate,
            IsHtml = request.Model.IsHtml
        };

        await emailTemplateManager.CreateEmailTemplateAsync(template, cancellationToken);
        return new CreatedModel { Id = template.Id };
    }
}
