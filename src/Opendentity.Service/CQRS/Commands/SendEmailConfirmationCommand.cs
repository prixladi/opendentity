using MediatR;

namespace Opendentity.Service.CQRS.Commands;

public record SendEmailConfirmationCommand(string Email): IRequest;
