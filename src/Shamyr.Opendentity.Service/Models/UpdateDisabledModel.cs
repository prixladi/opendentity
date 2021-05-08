using System.ComponentModel.DataAnnotations;

namespace Shamyr.Opendentity.Service.Models
{
    public record UpdateDisabledModel
    {
        [Required]
        public bool Disabled { get; init; }
    }
}
