using System;
using Shamyr.Opendentity.Database.Entities;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Shamyr.Opendentity.OpenId.Extensions
{
    public static class PayloadExtensions
    {
        public static ApplicationUser ToUser(this Payload payload)
        {
            if (payload is null)
                throw new ArgumentNullException(nameof(payload));

            return new ApplicationUser($"{payload.Email}-{Guid.NewGuid():N}")
            {
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                ImageUrl = payload.Picture,
                Email = payload.Email,
                EmailConfirmed = true
            };
        }
    }
}
