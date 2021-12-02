using FluentValidation;
using Opendentity.Service.Models;

namespace Opendentity.Service.Validation.Models;

public class CountModelValidator: AbstractValidator<CountModel>
{
    public CountModelValidator()
    {
        RuleFor(x => x.Count)
            .GreaterThanOrEqualTo(0);
    }
}
