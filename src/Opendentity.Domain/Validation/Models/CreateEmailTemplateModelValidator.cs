using FluentValidation;
using Opendentity.Domain.Models;

namespace Opendentity.Domain.Validation.Models;

public class CreateEmailTemplateModelValidator: AbstractValidator<CreateEmailTemplateModel>
{
    public CreateEmailTemplateModelValidator()
    {
        RuleFor(x => x.Subject)
            .NotEmpty();

        RuleFor(x => x.ContentTemplate)
            .NotEmpty();
    }
}
