using System;
using System.Collections.Generic;
using System.Linq;
using Opendentity.Database.Entities;
using Opendentity.Domain.Models;

namespace Opendentity.Domain.Extensions;

public static class ApplicationUserExtensions
{
    public static UserModel ToModel(this ApplicationUser user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        return new UserModel
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ImageUrl = user.ImageUrl
        };
    }

    public static ICollection<UserModel> ToModel(this IEnumerable<ApplicationUser> users)
    {
        if (users is null)
            throw new ArgumentNullException(nameof(users));

        return users.Select(x => x.ToModel()).ToArray();
    }
}
