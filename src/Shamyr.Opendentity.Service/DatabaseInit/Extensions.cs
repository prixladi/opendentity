using System.Collections.Generic;
using System.Linq;
using OpenIddict.Abstractions;
using Shamyr.Opendentity.Database.Entities;
using Shamyr.Opendentity.OpenId;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Shamyr.Opendentity.Service.DatabaseInit
{
    public static class Extensions
    {
        public static ApplicationUser ToApplicationUser(this UserInitDto dto)
        {
            if (dto is null)
                throw new System.ArgumentNullException(nameof(dto));

            return new ApplicationUser(dto.UserName)
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                ImageUrl = dto.ImageUrl,
                EmailConfirmed = true
            };
        }

        public static OpenIddictApplicationDescriptor ToDescriptor(this ApplicationInitDto dtos)
        {
            if (dtos is null)
                throw new System.ArgumentNullException(nameof(dtos));

            return new OpenIddictApplicationDescriptor
            {
                ClientId = dtos.ClientId,
                Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.RefreshToken,
                    Permissions.GrantTypes.Password,
                    CustomGrants._GooglePermission,
                    Permissions.Scopes.Address,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Phone,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles
                }
            };
        }

        public static EmailTemplate ToTemplate(this EmailTemplateDto dto, EmailTemplateType type)
        {
            if (dto is null)
                throw new System.ArgumentNullException(nameof(dto));

            return new EmailTemplate
            {
                Type = type,
                Subject = dto.Subject,
                ContentTemplate = dto.ContentTemplate,
                IsHtml = dto.IsHtml
            };
        }

        public static ICollection<OpenIddictApplicationDescriptor> ToDescriptor(this ICollection<ApplicationInitDto> dtos)
        {
            if (dtos is null)
                throw new System.ArgumentNullException(nameof(dtos));

            return dtos.Select(ToDescriptor).ToList();
        }
    }
}
