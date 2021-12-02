using MediatR;
using Opendentity.Service.Models;
using Opendentity.Service.RequestPipeline;

namespace Opendentity.Service.CQRS.Commands;

public record SetCurrentUserPasswordCommand(SetPasswordModel Model): IRequest, ITransactionRequest;
