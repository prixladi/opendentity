using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shamyr.Exceptions;
using Opendentity.Service.CQRS.Queries;
using Opendentity.Service.Models;
using Opendentity.Service.Services;

namespace Opendentity.Service.CQRS.Handlers;

public class GetEmailTemplateQueryHandler: IRequestHandler<GetEmailTemplateQuery, EmailTemplateModel>
{
    private readonly IEmailTemplateManager emailTemplateManager;

    public GetEmailTemplateQueryHandler(IEmailTemplateManager emailTemplateManager)
    {
        this.emailTemplateManager = emailTemplateManager;
    }

    public async Task<EmailTemplateModel> Handle(GetEmailTemplateQuery request, CancellationToken cancellationToken)
    {
        var emailTemplate = await emailTemplateManager.TryGetTemplateAsync(request.Id, cancellationToken);
        if (emailTemplate == null)
            throw new NotFoundException($"Email with id '{request.Id}' does not exist.");

        return new EmailTemplateModel
        {
            Id = emailTemplate.Id,
            Type = emailTemplate.Type,
            Subject = emailTemplate.Subject,
            ContentTemplate = emailTemplate.ContentTemplate,
            IsHtml = emailTemplate.IsHtml
        };
    }
}
