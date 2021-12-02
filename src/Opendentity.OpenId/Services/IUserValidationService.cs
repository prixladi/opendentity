namespace Opendentity.OpenId.Services;

public interface IUserValidationService
{
    void ValidateUsernameOrThrow(string username);
}
