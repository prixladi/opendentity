using MediatR;
using Opendentity.Service.Models;

namespace Opendentity.Service.CQRS.Commands;

public record UpdateCurrentUserCommand(UpdateUserModel Model): IRequest;
