using FluentValidation;
using Opendentity.Domain.Models;

namespace Opendentity.Domain.Validation.Models;

public class EmailTemplatePreviewModelValidator: AbstractValidator<EmailTemplatePreviewModel>
{
    public EmailTemplatePreviewModelValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(e => e.Subject)
            .NotEmpty();
    }
}
