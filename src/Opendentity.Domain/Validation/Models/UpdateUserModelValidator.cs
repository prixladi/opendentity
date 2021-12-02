using FluentValidation;
using Microsoft.Extensions.Options;
using Opendentity.Domain.CQRS.Users;
using Opendentity.Domain.Settings;

namespace Opendentity.Domain.Validation.Models;

public class UpdateUserModelValidator: AbstractValidator<UpdateUserModel>
{
    public UpdateUserModelValidator(IOptions<ValidationSettings> options)
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(options.Value.MinUserNameLength)
            .MaximumLength(options.Value.MaxUserNameLength);

        RuleFor(x => x.FirstName)
            .MaximumLength(options.Value.MaxNameLength);

        RuleFor(x => x.LastName)
            .MaximumLength(options.Value.MaxNameLength);

        RuleFor(x => x.ImageUrl)
            .Uri();
    }
}
