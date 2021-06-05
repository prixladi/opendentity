using MediatR;
using Shamyr.Opendentity.Service.Models;
using Shamyr.Opendentity.Service.RequestPipeline;

namespace Shamyr.Opendentity.Service.CQRS.Commands
{
    public record PasswordResetCommand(string Email, PasswordResetModel Model): IRequest, ITransactionRequest;
}
