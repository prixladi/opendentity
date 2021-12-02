using FluentValidation;
using Opendentity.Service.Models;

namespace Opendentity.Service.Validation.Models;

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
