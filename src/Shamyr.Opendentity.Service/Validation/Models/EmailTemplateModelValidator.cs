using FluentValidation;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.Validation.Models
{
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
}
