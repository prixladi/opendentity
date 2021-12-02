using MediatR;
using Opendentity.Service.Models;

namespace Opendentity.Service.CQRS.Commands;

public record ConfirmEmailCommand(string Email, TokenModel Model): IRequest;
