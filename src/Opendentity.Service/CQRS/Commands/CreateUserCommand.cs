using MediatR;
using Opendentity.Service.Models;

namespace Opendentity.Service.CQRS.Commands;

public record CreateUserCommand(CreateUserModel Model): IRequest<CreatedModel>;
