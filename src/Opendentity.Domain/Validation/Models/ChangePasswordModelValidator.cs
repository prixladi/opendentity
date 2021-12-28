using FluentValidation;
using Microsoft.Extensions.Options;
using Opendentity.Domain.Models;
using Opendentity.Domain.Settings;

namespace Opendentity.Domain.Validation.Models;

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
