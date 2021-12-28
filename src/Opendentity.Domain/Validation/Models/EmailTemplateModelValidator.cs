using FluentValidation;
using Opendentity.Domain.Models;

namespace Opendentity.Domain.Validation.Models;

public class EmailTemplateModelValidator: AbstractValidator<EmailTemplateModel>
{
    public EmailTemplateModelValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(e => e.Subject)
            .NotEmpty();

        RuleFor(e => e.ContentTemplate)
            .NotEmpty();
    }
}
