using AspNetCoreRateLimit;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Opendentity.Database;
using Opendentity.Database.DependencyInjection;
using Opendentity.Domain.DatabaseInit;
using Opendentity.Domain.Settings;
using Opendentity.Emails;
using Opendentity.Emails.DependencyInjection;
using Opendentity.OpenId;
using Opendentity.Service.Configs;
using Opendentity.Service.DependencyInjection;
using Shamyr.AspNetCore.DependencyInjection;
using Shamyr.AspNetCore.Configs;

using Constants = Opendentity.Service.Constants;
using Opendentity.OpenId.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
{
    services.AddMediatorPipeline();
    services.AddServiceAssembly();

    services.AddCors(CorsConfig.SetupAllowAny);
    services.AddControllers(MvcConfig.Setup)
        .AddJsonOptions(MvcConfig.SetupJson);

    services.AddDatabase()
        .ConfigureSettings<DatabaseSettings>(configuration.GetSection(Constants.SettingSections._Database));

    services.AddDatabaseInit()
        .ConfigureSettings<DatabaseInitSettings>(configuration.GetSection(Constants.SettingSections._DatabaseInit));

    services.AddOpenId()
        .ConfigureSettings<OpenIdSettings>(configuration.GetSection(Constants.SettingSections._OpenId));

    services.AddIdentity()
        .ConfigureSettings<IdentitySettings>(configuration.GetSection(Constants.SettingSections._Identity));

    services.AddEmailClient()
        .ConfigureSettings<EmailClientSettings>(configuration.GetSection(Constants.SettingSections._Email));

    services.Configure<UISettings>(configuration.GetSection(Constants.SettingSections._Ui));
    services.Configure<ValidationSettings>(configuration.GetSection(Constants.SettingSections._Validation));

    RateLimitConfig.Setup(services, configuration.GetSection(Constants.SettingSections._RateLimits));

    services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Program>());
    services.AddFluentValidationRulesToSwagger();

    services.AddLogging(LoggingConfig.Setup);
    services.AddExceptionHandling(typeof(Program).Assembly);
    services.AddSwaggerGen(SwaggerConfig.SetupSwaggerGen);
}

var app = builder.Build();
{
    app.UseCors(CorsConfig._AllowAnyCorsPolicy);

    app.UseSwagger();
    app.UseSwaggerUI(SwaggerConfig.SetupSwaggerUI);

    app.UseRouting();

    app.UseIpRateLimiting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}

app.Run();


