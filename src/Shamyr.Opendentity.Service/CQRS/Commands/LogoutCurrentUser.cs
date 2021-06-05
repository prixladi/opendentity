using MediatR;
using Shamyr.Opendentity.Service.RequestPipeline;

namespace Shamyr.Opendentity.Service.CQRS.Commands
{
    public record LogoutCurrentUser: IRequest, ITransactionRequest;
}
