using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shamyr.AspNetCore.Configs;
using Shamyr.Opendentity.Service.Configs;

namespace Shamyr.Opendentity.Service
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(CorsConfig.SetupAllowAny);
            services.AddControllers(MvcConfig.Setup)
                .AddJsonOptions(MvcConfig.SetupJson);

            services.AddDatabase<DatabaseConfig>()
                .AddDatabaseInit<DatabaseInitConfig>();

            services.AddOpenId(OpenIdConfig.Setup)
                .AddIdentity();

            services.AddLogging(LoggingConfig.Setup);

            services.AddMediatorPipeline();

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
