﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Opendentity.Database;
using Opendentity.Service.CQRS.Queries;
using Opendentity.Service.Models;

namespace Opendentity.Service.CQRS.Handlers;

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
