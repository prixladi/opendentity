using System.ComponentModel.DataAnnotations;

namespace Shamyr.Opendentity.Service.Models
{
    public record ChangePasswordModel
    {
        [Required]
        public string OldPassword { get; init; } = default!;

        [Required]
        public string NewPassword { get; init; } = default!;
    } 
}
