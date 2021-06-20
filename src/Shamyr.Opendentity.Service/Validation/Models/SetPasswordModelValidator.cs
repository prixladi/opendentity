using FluentValidation;
using Microsoft.Extensions.Options;
using Shamyr.Opendentity.Service.Models;
using Shamyr.Opendentity.Service.Settings;

namespace Shamyr.Opendentity.Service.Validation.Models
{
    public class SetPasswordModelValidator: AbstractValidator<SetPasswordModel>
    {
        public SetPasswordModelValidator(IOptions<ValidationSettings> options)
        {
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(options.Value.MinPasswordLength);
        }
    }
}
