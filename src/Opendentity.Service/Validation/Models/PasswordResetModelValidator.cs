using FluentValidation;
using Microsoft.Extensions.Options;
using Opendentity.Service.Models;
using Opendentity.Service.Settings;

namespace Opendentity.Service.Validation.Models;

public class PasswordResetModelValidator: AbstractValidator<PasswordResetModel>
{
    public PasswordResetModelValidator(IOptions<ValidationSettings> options)
    {
        RuleFor(x => x.Token)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(options.Value.MinPasswordLength);
    }
}
