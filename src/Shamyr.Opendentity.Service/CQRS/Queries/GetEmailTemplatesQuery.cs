using System.Collections.Generic;
using MediatR;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.CQRS.Queries
{
    public record GetEmailTemplatesQuery: IRequest<ICollection<EmailTemplatePreviewModel>>;
}
