using System.Collections.Generic;
using MediatR;
using Opendentity.Service.Models;

namespace Opendentity.Service.CQRS.Queries;

public record GetEmailTemplatesQuery: IRequest<ICollection<EmailTemplatePreviewModel>>;
