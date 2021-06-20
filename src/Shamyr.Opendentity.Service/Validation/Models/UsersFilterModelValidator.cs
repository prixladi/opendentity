using FluentValidation;
using Microsoft.Extensions.Options;
using Shamyr.Opendentity.Service.Models;
using Shamyr.Opendentity.Service.Settings;

namespace Shamyr.Opendentity.Service.Validation.Models
{
    public class UsersFilterModelValidator: AbstractValidator<UsersFilterModel>
    {
        public UsersFilterModelValidator(IOptions<ValidationSettings> options)
        {
            RuleFor(x => x.Offset)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Limit)
                .LessThanOrEqualTo(options.Value.MaxPageSize)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.Search)
                .MinimumLength(options.Value.MinSearchLength)
                .MaximumLength(options.Value.MaxSearchLength);
        }
    }
}
