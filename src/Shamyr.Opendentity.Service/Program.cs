using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Shamyr.Opendentity.Service;

await Host.CreateDefaultBuilder()
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
        webBuilder.UseKestrel();
    })
    .Build()
    .RunAsync();

