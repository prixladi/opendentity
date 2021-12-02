using MediatR;

namespace Opendentity.Service.CQRS.Commands;

public record SendPasswordResetCommand(string Email): IRequest;
