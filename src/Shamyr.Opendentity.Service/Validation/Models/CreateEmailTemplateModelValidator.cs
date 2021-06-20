using FluentValidation;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.Validation.Models
{
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
}
