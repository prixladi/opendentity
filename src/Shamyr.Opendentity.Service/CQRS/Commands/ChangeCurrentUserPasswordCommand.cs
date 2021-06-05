using MediatR;
using Shamyr.Opendentity.Service.Models;
using Shamyr.Opendentity.Service.RequestPipeline;

namespace Shamyr.Opendentity.Service.CQRS.Commands
{
    public record ChangeCurrentUserPasswordCommand(ChangePasswordModel Model): IRequest, ITransactionRequest;
}
