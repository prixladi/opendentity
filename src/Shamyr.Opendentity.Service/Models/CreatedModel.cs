using System.ComponentModel.DataAnnotations;

namespace Shamyr.Opendentity.Service.Models
{
    public record CreatedModel
    {
        [Required]
        public string Id { get; init; } = default!;
    }
}
