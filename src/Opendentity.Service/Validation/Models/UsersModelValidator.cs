using FluentValidation;
using Opendentity.Service.Models;

namespace Opendentity.Service.Validation.Models;

public class UsersModelValidator: AbstractValidator<UsersModel>
{
    public UsersModelValidator()
    {
        RuleFor(x => x.Total)
            .GreaterThanOrEqualTo(0);
    }
}
