using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CelestialObjectCatalog.WebApi.Converters;
using CelestialObjectCatalog.WebApi.IoC;

namespace CelestialObjectCatalog.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //set AutoFac as dependency container
            services.AddAutofac();

            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    //add options for displaying enums
                    options
                        .JsonSerializerOptions
                        .Converters
                        .Add(new JsonStringEnumConverter());

                    //add options for displaying datetime
                    options
                        .JsonSerializerOptions
                        .Converters
                        .Add(new DateTimeConverter());

                    //add options for displaying datetime
                    options
                        .JsonSerializerOptions
                        .Converters
                        .Add(new BigDecimalConverter());
                });

            services.AddSwaggerGen(c => c
                .SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "CelestialObjectCatalog.WebApi",
                        Version = "v1"
                    }));
        }

        /// <summary>
        /// This method is called at runtime for configuring all the dependencies
        /// </summary>
        /// <param name="containerBuilder">The AutoFac container builder</param>
        public void ConfigureContainer(ContainerBuilder containerBuilder)
            => WebApiBootstrapper.Bootstrap(containerBuilder);

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(
                c => c
                    .SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    "CelestialObjectCatalog.WebApi v1"));

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
