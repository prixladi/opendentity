using System;
using System.Collections.Generic;

namespace Opendentity.Domain.DatabaseInit;

public record RootInitDto
{
    public ICollection<UserInitDto> Users { get; init; } = Array.Empty<UserInitDto>();
    public ICollection<ApplicationInitDto> Applications { get; init; } = Array.Empty<ApplicationInitDto>();
    public EmailTemplateDto? PasswordResetEmail { get; init; }
    public EmailTemplateDto? ConfirmationEmail { get; init; }
}
