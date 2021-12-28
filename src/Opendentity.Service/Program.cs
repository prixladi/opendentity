using AspNetCoreRateLimit;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Opendentity.Domain;
using Opendentity.Domain.DependencyInjection;
using Opendentity.Service;
using Opendentity.Service.Configs;
using Opendentity.Service.DependencyInjection;
using Shamyr.AspNetCore.Configs;
using Shamyr.AspNetCore.DependencyInjection;
using Shamyr.AspNetCore.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
{
    builder.Services.AddCors(CorsConfig.SetupAllowAny);
    builder.Services.AddControllers(MvcConfig.Setup)
        .AddJsonOptions(MvcConfig.SetupJson);

    builder.Services.AddService(
        databaseInitSection: configuration.GetSection(Constants.SettingSections._DatabaseInit),
        identitySection: configuration.GetSection(Constants.SettingSections._Identity),
        rateLimitSection: configuration.GetSection(Constants.SettingSections._RateLimits));

    builder.Services.AddDomain(
        databaseSection: configuration.GetSection(Constants.SettingSections._Database),
        openIdSection: configuration.GetSection(Constants.SettingSections._OpenId),
        emailClientSection: configuration.GetSection(Constants.SettingSections._Email),
        uiSection: configuration.GetSection(Constants.SettingSections._Ui),
        validationSection: configuration.GetSection(Constants.SettingSections._Validation));

    builder.Services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(DomainConstants)));
    builder.Services.AddFluentValidationRulesToSwagger();

    builder.Services.AddLogging(LoggingConfig.Setup);
    builder.Services.AddExceptionHandling(typeof(Program).Assembly);
    builder.Services.AddSwaggerGen(SwaggerConfig.SetupSwaggerGen);
}

var app = builder.Build();
{
    app.UseCors(CorsConfig._AllowAnyCorsPolicy);

    app.UseRouting();

    app.UseExceptionHandling();

    app.UseSwagger();
    app.UseSwaggerUI(SwaggerConfig.SetupSwaggerUI);

    app.UseIpRateLimiting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}

app.Run();
