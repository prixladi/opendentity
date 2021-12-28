using FluentValidation;
using Opendentity.Domain.Models;

namespace Opendentity.Domain.Validation.Models;

public class UserModelValidator: AbstractValidator<UserModel>
{
    public UserModelValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.ImageUrl)
            .Uri();
    }
}
