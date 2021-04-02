using Microsoft.AspNetCore.Hosting;
using Shamyr.Opendentity.Service;

await new WebHostBuilder()
  .UseKestrel()
  .UseStartup<Startup>()
  .Build()
  .RunAsync();

