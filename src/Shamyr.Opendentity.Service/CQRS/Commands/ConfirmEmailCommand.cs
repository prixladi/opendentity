using MediatR;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.CQRS.Commands
{
    public record ConfirmEmailCommand(string Email, TokenModel Model): IRequest;
}
