﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shamyr.AspNetCore;
using Shamyr.AspNetCore.HttpErrors;

namespace Opendentity.Service.Configs;

public static class MvcConfig
{
    public static void Setup(MvcOptions options)
    {
        options.Filters.Add(new ProducesResponseTypeAttribute(typeof(HttpErrorResponseModel), StatusCodes.Status400BadRequest));
        options.Filters.Add(new ProducesResponseTypeAttribute(typeof(HttpErrorResponseModel), StatusCodes.Status429TooManyRequests));
        options.Filters.Add(new ProducesResponseTypeAttribute(typeof(HttpErrorResponseModel), StatusCodes.Status500InternalServerError));
    }

    public static void SetupJson(JsonOptions options)
    {
        Json.SetupDefaultSerializerOptions(options.JsonSerializerOptions);
    }
}
