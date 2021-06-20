using MediatR;
using Shamyr.Opendentity.Service.Models;
using Shamyr.Opendentity.Service.Validation.Models;

namespace Shamyr.Opendentity.Service.CQRS.Commands
{
    public record CreateUserCommand(CreateUserModel Model): IRequest<CreatedModel>;
}
