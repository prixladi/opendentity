using FluentValidation;
using Microsoft.Extensions.Options;
using Opendentity.Service.Models;
using Opendentity.Service.Settings;

namespace Opendentity.Service.Validation.Models;

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
