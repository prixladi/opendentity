using FluentValidation;
using Opendentity.Domain.Models;

namespace Opendentity.Domain.Validation.Models;

public class UsersModelValidator: AbstractValidator<UsersModel>
{
    public UsersModelValidator()
    {
        RuleFor(x => x.Total)
            .GreaterThanOrEqualTo(0);
    }
}
