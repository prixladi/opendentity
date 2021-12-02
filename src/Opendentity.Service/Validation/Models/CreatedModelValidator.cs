using FluentValidation;
using Opendentity.Service.Models;

namespace Opendentity.Service.Validation.Models;

public class CreatedModelValidator: AbstractValidator<CreatedModel>
{
    public CreatedModelValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
