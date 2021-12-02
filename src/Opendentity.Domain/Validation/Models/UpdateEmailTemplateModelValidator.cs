using FluentValidation;
using Opendentity.Domain.Models;

namespace Opendentity.Domain.Validation.Models;

public class UpdateEmailTemplateModelValidator: AbstractValidator<UpdateEmailTemplateModel>
{
    public UpdateEmailTemplateModelValidator()
    {
        RuleFor(x => x.Subject)
            .NotEmpty();

        RuleFor(x => x.ContentTemplate)
            .NotEmpty();
    }
}
