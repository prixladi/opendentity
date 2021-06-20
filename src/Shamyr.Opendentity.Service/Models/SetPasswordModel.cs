using System.ComponentModel.DataAnnotations;

namespace Shamyr.Opendentity.Service.Models
{
    public record SetPasswordModel
    {
        public string Password { get; init; } = default!;
    }
}
