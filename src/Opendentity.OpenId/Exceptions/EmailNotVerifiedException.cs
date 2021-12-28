using System;

namespace Opendentity.OpenId.Exceptions;

public class EmailNotVerifiedException: Exception
{
    public string Email { get; }

    public EmailNotVerifiedException(string email)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
    }
}
