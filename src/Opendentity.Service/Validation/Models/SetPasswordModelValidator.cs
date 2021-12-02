using FluentValidation;
using Microsoft.Extensions.Options;
using Opendentity.Service.Models;
using Opendentity.Service.Settings;

namespace Opendentity.Service.Validation.Models;

public class SetPasswordModelValidator: AbstractValidator<SetPasswordModel>
{
    public SetPasswordModelValidator(IOptions<ValidationSettings> options)
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(options.Value.MinPasswordLength);
    }
}
