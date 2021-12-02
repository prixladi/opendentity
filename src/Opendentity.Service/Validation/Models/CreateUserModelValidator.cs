using FluentValidation;
using Microsoft.Extensions.Options;
using Opendentity.Service.Models;
using Opendentity.Service.Settings;

namespace Opendentity.Service.Validation.Models;

public class CreateUserModelValidator: AbstractValidator<CreateUserModel>
{
    public CreateUserModelValidator(IOptions<ValidationSettings> options)
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(options.Value.MinUserNameLength)
            .MaximumLength(options.Value.MaxUserNameLength);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(options.Value.MaxEmailLength);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(options.Value.MinPasswordLength);

        RuleFor(x => x.FirstName)
            .MaximumLength(options.Value.MaxNameLength);

        RuleFor(x => x.LastName)
            .MaximumLength(options.Value.MaxNameLength);

        RuleFor(x => x.ImageUrl)
            .Uri();
    }
}
