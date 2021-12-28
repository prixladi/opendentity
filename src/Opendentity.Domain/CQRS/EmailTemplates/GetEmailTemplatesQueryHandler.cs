using MediatR;
using Microsoft.EntityFrameworkCore;
using Opendentity.Database;
using Opendentity.Domain.Models;

namespace Opendentity.Domain.CQRS.EmailTemplates;

public record GetEmailTemplatesQuery: IRequest<ICollection<EmailTemplatePreviewModel>>;

public class GetEmailTemplatesQueryHandler: IRequestHandler<GetEmailTemplatesQuery, ICollection<EmailTemplatePreviewModel>>
{
    private readonly DatabaseContext databaseContext;

    public GetEmailTemplatesQueryHandler(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    public async Task<ICollection<EmailTemplatePreviewModel>> Handle(GetEmailTemplatesQuery request, CancellationToken cancellationToken)
    {
        return await databaseContext.EmailTemplates
            .Select(e => new EmailTemplatePreviewModel
            {
                Id = e.Id,
                Type = e.Type,
                Subject = e.Subject,
                IsHtml = e.IsHtml
            })
            .ToArrayAsync(cancellationToken);
    }
}
