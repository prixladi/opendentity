using FluentValidation;
using Opendentity.Domain.Models;

namespace Opendentity.Domain.Validation.Models;

public class TokenModelValidator: AbstractValidator<TokenModel>
{
    public TokenModelValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
