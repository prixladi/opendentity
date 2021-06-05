using MediatR;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.CQRS.Commands
{
    public record CreateEmailTemplateCommand(CreateEmailTemplateModel Model): IRequest<CreatedModel>;
}
