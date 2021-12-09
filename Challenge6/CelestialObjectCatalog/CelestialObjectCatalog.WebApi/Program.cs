using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CelestialObjectCatalog.WebApi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            //configure logger
            Log.Logger = new LoggerConfiguration()
                .Enrich
                .FromLogContext()
                .WriteTo
                .Console()
                .CreateLogger();

            //start executing app
            try
            {
                //log starting message
                Log.Information("Starting up...");

                //run app
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                //registered the service provider to be AutoFac container
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
