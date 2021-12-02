using MediatR;
using Opendentity.Service.Models;
using Opendentity.Service.RequestPipeline;

namespace Opendentity.Service.CQRS.Commands;

public record PasswordResetCommand(string Email, PasswordResetModel Model): IRequest, ITransactionRequest;
