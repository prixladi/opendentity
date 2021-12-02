using FluentValidation;
using Opendentity.Service.Models;

namespace Opendentity.Service.Validation.Models;

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
