using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shamyr.AspNetCore.HttpErrors;

namespace Shamyr.Opendentity.Service.Configs
{
    public static class MvcConfig
    {
        public static void Setup(MvcOptions options)
        {
            options.Filters.Add<HttpExceptionFilter>();
            options.Filters.Add(new ProducesResponseTypeAttribute(typeof(HttpErrorResponseModel), StatusCodes.Status400BadRequest));
            options.Filters.Add(new ProducesResponseTypeAttribute(typeof(HttpErrorResponseModel), StatusCodes.Status500InternalServerError));
        }

        public static void SetupJson(JsonOptions options)
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        }
    }
}
