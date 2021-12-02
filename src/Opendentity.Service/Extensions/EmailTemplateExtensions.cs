﻿using System;
using Opendentity.Database.Entities;
using Opendentity.Emails;

namespace Opendentity.Service.Extensions;

public static class EmailTemplateExtensions
{
    public static EmailDataDto ToPasswordResetEmail(this EmailTemplate template, string token, string email, string portalUrl)
    {
        if (template.Type != EmailTemplateType.PasswordResetEmail)
            throw new InvalidOperationException($"Template of type '{template.Type}' cannot be transformed to email of type {EmailTemplateType.PasswordResetEmail}.");

        var content = template.ContentTemplate
            .Replace(EmailConstants.ReplacementMarks.PasswordReset._PasswordTokenMark, token)
            .Replace(EmailConstants.ReplacementMarks.PasswordReset._EmailMark, email)
            .Replace(EmailConstants.ReplacementMarks.PasswordReset._PortalUrlMark, portalUrl);

        return new EmailDataDto
        {
            Subject = template.Subject,
            Body = new EmailBodyDto(content, template.IsHtml)
        };
    }

    public static EmailDataDto ToConfirmationEmail(this EmailTemplate template, string token, string email, string portalUrl)
    {
        if (template.Type != EmailTemplateType.ConfirmationEmail)
            throw new InvalidOperationException($"Template of type '{template.Type}' cannot be transformed to email of type {EmailTemplateType.ConfirmationEmail}.");

        var content = template.ContentTemplate
            .Replace(EmailConstants.ReplacementMarks.Confirmation._VerifyTokenMark, token)
            .Replace(EmailConstants.ReplacementMarks.Confirmation._EmailMark, email)
            .Replace(EmailConstants.ReplacementMarks.Confirmation._PortalUrlMark, portalUrl);

        return new EmailDataDto
        {
            Subject = template.Subject,
            Body = new EmailBodyDto(content, template.IsHtml)
        };
    }
}
