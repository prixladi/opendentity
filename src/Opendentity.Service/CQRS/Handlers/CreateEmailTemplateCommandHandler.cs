using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Opendentity.Database;
using Shamyr.Exceptions;
using Opendentity.Database.Entities;
using Opendentity.Service.CQRS.Commands;
using Opendentity.Service.Models;
using Opendentity.Service.Services;

namespace Opendentity.Service.CQRS.Handlers;

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
