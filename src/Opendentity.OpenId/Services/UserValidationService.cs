using System;
using System.Linq;
using Shamyr.Exceptions;

namespace Opendentity.OpenId.Services;

public class UserValidationService: IUserValidationService
{
    public void ValidateUsernameOrThrow(string username)
    {
        if (username is null)
            throw new ArgumentNullException(nameof(username));

        if (!username.All(x => Utils._AllowedUsernameCharacters.Contains(x)))
            throw new BadRequestException($"Username can only contain following characters '{Utils._AllowedUsernameCharacters}'.");
    }
}
