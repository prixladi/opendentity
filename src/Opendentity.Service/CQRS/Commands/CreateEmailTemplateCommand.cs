using MediatR;
using Opendentity.Service.Models;

namespace Opendentity.Service.CQRS.Commands;

public record CreateEmailTemplateCommand(CreateEmailTemplateModel Model): IRequest<CreatedModel>;
