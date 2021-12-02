using FluentValidation;
using Opendentity.Service.Models;

namespace Opendentity.Service.Validation.Models;

public class TokenModelValidator: AbstractValidator<TokenModel>
{
    public TokenModelValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
