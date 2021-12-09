using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Autofac;
using Autofac.Extensions.DependencyInjection;
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

            services.AddControllers();

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
        /// Used modules for better separation of concerns in DI
        /// </summary>
        /// <param name="containerBuilder">The AutoFac container builder</param>
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            //register persistence module
            containerBuilder
                .RegisterModule<PersistenceModule>();

            //register the classification engine module
            containerBuilder
                .RegisterModule<ClassificationEngineModule>();

            //register service module
            containerBuilder
                .RegisterModule<ServiceModule>();
        }

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
