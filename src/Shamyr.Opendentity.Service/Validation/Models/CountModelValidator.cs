using FluentValidation;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.Validation.Models
{
    public class CountModelValidator: AbstractValidator<CountModel>
    {
        public CountModelValidator()
        {
            RuleFor(x => x.Count)
                .GreaterThanOrEqualTo(0);
        }
    }
}
