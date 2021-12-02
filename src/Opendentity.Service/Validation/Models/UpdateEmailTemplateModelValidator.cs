using FluentValidation;
using Opendentity.Service.Models;

namespace Opendentity.Service.Validation.Models;

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
