using System.ComponentModel.DataAnnotations;

namespace Shamyr.Opendentity.Service.Models
{
    public record TokenModel
    {
        [Required]
        public string Token { get; init; } = default!;
    }
}
