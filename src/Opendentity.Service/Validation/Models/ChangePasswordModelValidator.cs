using FluentValidation;
using Microsoft.Extensions.Options;
using Opendentity.Service.Models;
using Opendentity.Service.Settings;

namespace Opendentity.Service.Validation.Models;

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
