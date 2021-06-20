namespace Shamyr.Opendentity.Service.Models
{
    public record ChangePasswordModel
    {
        public string OldPassword { get; init; } = default!;
        public string NewPassword { get; init; } = default!;
    }
}
