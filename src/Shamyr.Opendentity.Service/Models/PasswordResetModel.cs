using System.ComponentModel.DataAnnotations;

namespace Shamyr.Opendentity.Service.Models
{
    public record PasswordResetModel
    {
        [Required]
        public string Token { get; init; } = default!;

        [Required]
        public string Password { get; init; } = default!;
    }
}
