using MediatR;
using Shamyr.Opendentity.Service.Models;
using Shamyr.Opendentity.Service.RequestPipeline;

namespace Shamyr.Opendentity.Service.CQRS.Commands
{
    public record SetCurrentUserPasswordCommand(SetPasswordModel Model): IRequest, ITransactionRequest;
}
