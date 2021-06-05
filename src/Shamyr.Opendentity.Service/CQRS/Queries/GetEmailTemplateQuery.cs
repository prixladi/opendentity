using MediatR;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.CQRS.Queries
{
    public record GetEmailTemplateQuery(string Id): IRequest<EmailTemplateModel>;
}
