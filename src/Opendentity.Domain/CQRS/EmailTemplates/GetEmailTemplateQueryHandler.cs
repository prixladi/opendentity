using MediatR;
using Opendentity.Domain.Models;
using Opendentity.Domain.Services;
using Shamyr.Exceptions;

namespace Opendentity.Domain.CQRS;

public record GetEmailTemplateQuery(string Id): IRequest<EmailTemplateModel>;

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
