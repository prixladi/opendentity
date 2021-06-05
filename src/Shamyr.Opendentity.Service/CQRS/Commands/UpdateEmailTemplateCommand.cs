using MediatR;
using Shamyr.Opendentity.Service.Models;
using Shamyr.Opendentity.Service.RequestPipeline;

namespace Shamyr.Opendentity.Service.CQRS.Commands
{
    public record UpdateEmailTemplateCommand(string Id, UpdateEmailTemplateModel Model): ITransactionRequest, IRequest;
}
