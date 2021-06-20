using FluentValidation;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.Validation.Models
{
    public class CreatedModelValidator: AbstractValidator<CreatedModel>
    {
        public CreatedModelValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}
