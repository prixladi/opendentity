using FluentValidation;
using Microsoft.Extensions.Options;
using Shamyr.Opendentity.Service.Models;
using Shamyr.Opendentity.Service.Settings;

namespace Shamyr.Opendentity.Service.Validation.Models
{
    public class ChangePasswordModelValidator: AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordModelValidator(IOptions<ValidationSettings> options)
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(options.Value.MinPasswordLength);
        }
    }
}
