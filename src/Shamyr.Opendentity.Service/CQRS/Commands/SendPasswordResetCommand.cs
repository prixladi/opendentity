using MediatR;

namespace Shamyr.Opendentity.Service.CQRS.Commands
{
    public record SendPasswordResetCommand(string Email): IRequest;
}
