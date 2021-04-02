using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Shamyr.Opendentity.Service.OpenId;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Shamyr.Opendentity.Service.Configs
{
    public static class SwaggerConfig
    {
        private const string _V1Route = "v1";
        private const string _V1Title = "Opendentity api V1";

        public static void SetupSwaggerUI(SwaggerUIOptions options)
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", options.DocumentTitle);
        }

        public static void SetupSwaggerGen(SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("auth", new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Password = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri(OpenIdConstants._TokenRoute, UriKind.Relative)
                    }
                }
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
              {
                new OpenApiSecurityScheme
                {
                  Reference = new OpenApiReference
                  {
                    Id = "auth",
                    Type = ReferenceType.SecurityScheme
                  }
                },
                Array.Empty<string>()
              }
            });

            options.SwaggerDoc(_V1Route, new OpenApiInfo { Title = _V1Title, Version = _V1Route });
            options.DocInclusionPredicate((version, apiDescription) => apiDescription.RelativePath.Contains($"/{version}/"));

            foreach (string xmlFile in Directory.GetFiles(PlatformServices.Default.Application.ApplicationBasePath, "*.xml", SearchOption.AllDirectories))
                options.IncludeXmlComments(xmlFile);
        }
    }
}