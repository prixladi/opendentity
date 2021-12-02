using FluentValidation;

namespace Opendentity.Domain.Validation;

public static class RuleBuilderOptionsExtensions
{
    public static IRuleBuilderOptions<TObj, string?> Uri<TObj>(this IRuleBuilderInitial<TObj, string?> opt)
    {
        static bool MustBeAUri(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return true;
            }

            return System.Uri.TryCreate(url, UriKind.Absolute, out _);
        }

        return opt.Must(MustBeAUri)
           .WithMessage("Link '{PropertyValue}' must be a valid URI. eg: http://www.SomeWebSite.com");
    }
}
