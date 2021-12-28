using FluentValidation;
using Microsoft.Extensions.Options;
using Opendentity.Domain.Models;
using Opendentity.Domain.Settings;

namespace Opendentity.Domain.Validation.Models;

public class SetPasswordModelValidator: AbstractValidator<SetPasswordModel>
{
    public SetPasswordModelValidator(IOptions<ValidationSettings> options)
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(options.Value.MinPasswordLength);
    }
}
