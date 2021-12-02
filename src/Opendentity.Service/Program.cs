using AspNetCoreRateLimit;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Opendentity.Database;
using Opendentity.Database.DependencyInjection;
using Opendentity.Domain;
using Opendentity.Domain.DatabaseInit;
using Opendentity.Domain.Settings;
using Opendentity.Emails;
using Opendentity.Emails.DependecyInjection;
using Opendentity.OpenId;
using Opendentity.OpenId.DependencyInjection;
using Opendentity.Service;
using Opendentity.Service.Configs;
using Opendentity.Service.DependencyInjection;
using Shamyr.AspNetCore.Configs;
using Shamyr.AspNetCore.DependencyInjection;
using Shamyr.AspNetCore.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
{
    builder.Services.AddMediatorPipeline();
    builder.Services.AddServiceAssembly();

    builder.Services.AddCors(CorsConfig.SetupAllowAny);
    builder.Services.AddControllers(MvcConfig.Setup)
        .AddJsonOptions(MvcConfig.SetupJson);

    builder.Services.AddDatabase()
        .ConfigureSettings<DatabaseSettings>(configuration.GetSection(Constants.SettingSections._Database));

    builder.Services.AddDatabaseInit()
        .ConfigureSettings<DatabaseInitSettings>(configuration.GetSection(Constants.SettingSections._DatabaseInit));

    builder.Services.AddOpenId()
        .ConfigureSettings<OpenIdSettings>(configuration.GetSection(Constants.SettingSections._OpenId));

    builder.Services.AddIdentity()
        .ConfigureSettings<IdentitySettings>(configuration.GetSection(Constants.SettingSections._Identity));

    builder.Services.AddEmailClient()
        .ConfigureSettings<EmailClientSettings>(configuration.GetSection(Constants.SettingSections._Email));

    builder.Services.Configure<UISettings>(configuration.GetSection(Constants.SettingSections._Ui));
    builder.Services.Configure<ValidationSettings>(configuration.GetSection(Constants.SettingSections._Validation));

    RateLimitConfig.Setup(builder.Services, configuration.GetSection(Constants.SettingSections._RateLimits));

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
