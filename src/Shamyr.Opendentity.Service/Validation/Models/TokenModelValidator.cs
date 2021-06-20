using FluentValidation;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.Validation.Models
{
    public class TokenModelValidator: AbstractValidator<TokenModel>
    {
        public TokenModelValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty();
        }
    }
}
