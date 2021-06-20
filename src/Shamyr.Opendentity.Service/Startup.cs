using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shamyr.AspNetCore.Configs;
using Shamyr.Opendentity.Database;
using Shamyr.Opendentity.Emails;
using Shamyr.Opendentity.OpenId;
using Shamyr.Opendentity.Service.Configs;
using Shamyr.Opendentity.Service.DatabaseInit;
using Shamyr.Opendentity.Service.Settings;

namespace Shamyr.Opendentity.Service
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private RedisSettings RedisSettings => configuration.GetSection(Constants.SettingSections._Redis).Get<RedisSettings>();

        public void ConfigureServices(IServiceCollection services)
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
                .ConfigureSettings<IdentityOptions>(configuration.GetSection(Constants.SettingSections._Identity));

            services.AddEmailClient()
                .Configure<EmailClientSettings>(configuration.GetSection(Constants.SettingSections._Email));

            services.Configure<UISettings>(configuration.GetSection(Constants.SettingSections._Ui));
            services.Configure<ValidationSettings>(configuration.GetSection(Constants.SettingSections._Validation));

            DistributedCacheConfig.SetupWithRedis(services, RedisSettings);

            services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddFluentValidationRulesToSwagger();

            services.AddLogging(LoggingConfig.Setup);
            services.AddExceptionHandling(typeof(Startup).Assembly);
            services.AddSwaggerGen(SwaggerConfig.SetupSwaggerGen);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors(CorsConfig._AllowAnyCorsPolicy);

            app.UseSwagger();
            app.UseSwaggerUI(SwaggerConfig.SetupSwaggerUI);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
