using System.ComponentModel.DataAnnotations;

namespace Shamyr.Opendentity.Service.Models
{
    public record UpdateDisabledModel
    {
        public bool Disabled { get; init; }
    }
}
