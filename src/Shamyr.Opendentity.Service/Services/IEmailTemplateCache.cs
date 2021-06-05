using System;
using Shamyr.Opendentity.Database.Entities;

namespace Shamyr.Opendentity.Service.Services
{
    public interface IEmailTemplateCache
    {
        void AddOrUpdate(EmailTemplateType type, EmailTemplate template);
        bool TryGet(EmailTemplateType type, out EmailTemplate? value);
        bool TryRemove(EmailTemplateType type);
    }
}