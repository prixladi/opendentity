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

            // TODO: Add email after user validator allowes non-alphanumeric characters
            return new ApplicationUser(Guid.NewGuid().ToString("N"))
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
