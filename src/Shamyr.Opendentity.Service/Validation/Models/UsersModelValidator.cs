using FluentValidation;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.Validation.Models
{
    public class UsersModelValidator: AbstractValidator<UsersModel>
    {
        public UsersModelValidator()
        {
            RuleFor(x => x.Total)
                .GreaterThanOrEqualTo(0);
        }
    }
}
