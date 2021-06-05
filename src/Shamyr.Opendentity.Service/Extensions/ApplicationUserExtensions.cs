using System;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.Extensions
{
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
    }
}
