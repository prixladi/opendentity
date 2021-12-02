using MediatR;

namespace Opendentity.Service.CQRS.Commands;

public record DeleteEmailTemplateCommand(string Key): IRequest;
