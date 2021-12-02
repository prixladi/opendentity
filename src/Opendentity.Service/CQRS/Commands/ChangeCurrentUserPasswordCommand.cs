using MediatR;
using Opendentity.Service.Models;
using Opendentity.Service.RequestPipeline;

namespace Opendentity.Service.CQRS.Commands;

public record ChangeCurrentUserPasswordCommand(ChangePasswordModel Model): IRequest, ITransactionRequest;
