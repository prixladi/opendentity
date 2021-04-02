using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shamyr.AspNetCore.Configs;
using Shamyr.Opendentity.Service.Configs;
using Shamyr.Opendentity.Service.HostedServices;

namespace Shamyr.Opendentity.Service
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(CorsConfig.SetupAllowAny);
            services.AddControllers(MvcConfig.Setup)
                .AddJsonOptions(MvcConfig.SetupJson);

            services.AddDatabase(EnvVariable.Get(EnvVariables._DatabaseConnectionString));
            services.AddHostedService<DbInitializationHostedService>();

            services.AddOpenId(OpenIdConfig.Setup)
                .AddIdentity();

            services.AddLogging(LoggingConfig.Setup);
            services.AddMediatR(typeof(Startup));
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
