﻿using MediatR;
using Opendentity.Domain.Services;
using Shamyr.Exceptions;

namespace Opendentity.Domain.CQRS.EmailTemplates;

public record DeleteEmailTemplateCommand(string Key): IRequest;

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
