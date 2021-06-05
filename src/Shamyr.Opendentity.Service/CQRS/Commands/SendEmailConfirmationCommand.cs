using MediatR;

namespace Shamyr.Opendentity.Service.CQRS.Commands
{
    public record SendEmailConfirmationCommand(string Email): IRequest;
}
