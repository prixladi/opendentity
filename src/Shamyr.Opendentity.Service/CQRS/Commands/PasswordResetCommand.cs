using MediatR;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.CQRS.Commands
{
    public record PasswordResetCommand(string Email, PasswordResetModel Model): IRequest { }
}
