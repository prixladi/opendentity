using FluentValidation;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.Validation.Models
{
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
}
