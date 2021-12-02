using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shamyr.Exceptions;
using Opendentity.Service.CQRS.Commands;
using Opendentity.Service.Services;

namespace Opendentity.Service.CQRS.Handlers;

public class DeleteEmailTemplateCommandHandler: IRequestHandler<DeleteEmailTemplateCommand>
{
    private readonly IEmailTemplateManager emailTemplateManager;

    public DeleteEmailTemplateCommandHandler(IEmailTemplateManager emailTemplateManager)
    {
        this.emailTemplateManager = emailTemplateManager;
    }

    public async Task<Unit> Handle(DeleteEmailTemplateCommand request, CancellationToken cancellationToken)
    {
        if (!await emailTemplateManager.RemoveEmailTemplateAsync(request.Key, cancellationToken))
            throw new NotFoundException($"Email with key '{request.Key}' does not exist.");

        return Unit.Value;
    }
}
