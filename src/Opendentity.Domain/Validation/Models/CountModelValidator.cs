using FluentValidation;
using Opendentity.Domain.Models;

namespace Opendentity.Domain.Validation.Models;

public class CountModelValidator: AbstractValidator<CountModel>
{
    public CountModelValidator()
    {
        RuleFor(x => x.Count)
            .GreaterThanOrEqualTo(0);
    }
}
