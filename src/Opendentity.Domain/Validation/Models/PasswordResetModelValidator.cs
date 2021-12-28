using FluentValidation;
using Microsoft.Extensions.Options;
using Opendentity.Domain.Models;
using Opendentity.Domain.Settings;

namespace Opendentity.Domain.Validation.Models;

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
