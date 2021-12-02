using MediatR;
using Opendentity.Service.Models;
using Opendentity.Service.RequestPipeline;

namespace Opendentity.Service.CQRS.Commands;

public record UpdateUserDisabledCommand(string Id, UpdateDisabledModel Model): IRequest, ITransactionRequest;
