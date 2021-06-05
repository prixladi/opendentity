using System;
using System.Collections.Concurrent;
using Shamyr.Extensions.DependencyInjection;
using Shamyr.Opendentity.Database.Entities;

namespace Shamyr.Opendentity.Service.Services
{
    [Singleton]
    public class EmailTemplateCache: IEmailTemplateCache
    {
        private record EmailTemplateDto
        {
            public string Id { get; set; }
            public string Subject { get; set; } = default!;
            public string ContentTemplate { get; set; } = default!;
            public bool IsHtml { get; set; }

            public EmailTemplateDto(EmailTemplate template)
            {
                Id = template.Id;
                Subject = template.Subject;
                ContentTemplate = template.ContentTemplate;
                IsHtml = template.IsHtml;
            }

            public EmailTemplate ToTemplate(EmailTemplateType type)
            {
                return new EmailTemplate
                {
                    Id = Id,
                    Type = type,
                    Subject = Subject,
                    ContentTemplate = ContentTemplate,
                    IsHtml = IsHtml
                };
            }
        }

        private readonly ConcurrentDictionary<EmailTemplateType, EmailTemplateDto> templates;

        public EmailTemplateCache()
        {
            templates = new ConcurrentDictionary<EmailTemplateType, EmailTemplateDto>();
        }

        public bool TryGet(EmailTemplateType type, out EmailTemplate? value)
        {
            if (templates.TryGetValue(type, out var dto))
            {
                value = dto.ToTemplate(type);
                return true;
            }

            value = null;
            return false;
        }

        public void AddOrUpdate(EmailTemplateType type,EmailTemplate template)
        {
            templates.AddOrUpdate(type, new EmailTemplateDto(template), (_, __) => new EmailTemplateDto(template));
        }

        public bool TryRemove(EmailTemplateType type)
        {
            return templates.TryRemove(type, out _);
        }
    }
}
