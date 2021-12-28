namespace Opendentity.Domain.Models;

public record UsersModel
{
    public int Total { get; init; }
    public ICollection<UserModel> Users { get; init; } = Array.Empty<UserModel>();
}
