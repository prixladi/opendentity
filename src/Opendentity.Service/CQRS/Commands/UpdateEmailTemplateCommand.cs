using MediatR;
using Opendentity.Service.Models;
using Opendentity.Service.RequestPipeline;

namespace Opendentity.Service.CQRS.Commands;

public record UpdateEmailTemplateCommand(string Id, UpdateEmailTemplateModel Model): ITransactionRequest, IRequest;
