using FluentValidation;
using Opendentity.Domain.Models;

namespace Opendentity.Domain.Validation.Models;

public class CreatedModelValidator: AbstractValidator<CreatedModel>
{
    public CreatedModelValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
