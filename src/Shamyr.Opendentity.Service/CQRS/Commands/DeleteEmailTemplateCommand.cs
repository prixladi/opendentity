using MediatR;

namespace Shamyr.Opendentity.Service.CQRS.Commands
{
    public record DeleteEmailTemplateCommand(string Key): IRequest;
}
