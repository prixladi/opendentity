using MediatR;
using Microsoft.EntityFrameworkCore;
using Opendentity.Database;
using Opendentity.Domain.Models;
using Opendentity.Domain.RequestPipeline;
using Opendentity.Domain.Services;
using Shamyr.Exceptions;

namespace Opendentity.Domain.CQRS;

public record UpdateEmailTemplateCommand(string Id, UpdateEmailTemplateModel Model): ITransactionRequest, IRequest;

public class UpdateEmailTemplateCommandHandler: IRequestHandler<UpdateEmailTemplateCommand>
{
    private readonly IEmailTemplateManager emailTemplateManager;
    private readonly DatabaseContext databaseContext;

    public UpdateEmailTemplateCommandHandler(IEmailTemplateManager emailTemplateManager, DatabaseContext databaseContext)
    {
        this.emailTemplateManager = emailTemplateManager;
        this.databaseContext = databaseContext;
    }

    public async Task<Unit> Handle(UpdateEmailTemplateCommand request, CancellationToken cancellationToken)
    {
        var emailTemplate = await emailTemplateManager.TryGetTemplateAsync(request.Id, cancellationToken);
        if (emailTemplate == null)
            throw new NotFoundException($"Email with key '{request.Id}' does not exist.");

        var oldType = emailTemplate.Type;

        if (request.Model.Type is not null && await databaseContext.EmailTemplates.AnyAsync(e => e.Type == request.Model.Type, cancellationToken))
        {
            throw new ConflictException($"Email template with ID '{request.Model.Type}' already exist.");
        }

        emailTemplate.Type = request.Model.Type;
        emailTemplate.Subject = request.Model.Subject;
        emailTemplate.ContentTemplate = request.Model.ContentTemplate;
        emailTemplate.IsHtml = request.Model.IsHtml;

        await emailTemplateManager.UpdateEmailTemplateAsync(emailTemplate, oldType, cancellationToken);

        return Unit.Value;
    }
}
