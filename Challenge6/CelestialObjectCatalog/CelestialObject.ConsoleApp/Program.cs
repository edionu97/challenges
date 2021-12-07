using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Autofac;
using CelestialObjectCatalog.Persistence.Context;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Models.Enums;
using CelestialObjectCatalog.Persistence.Repository.Abstract;
using CelestialObjectCatalog.Utility.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CelestialObject.ConsoleApp
{

    public class BloggingContextFactory : IDesignTimeDbContextFactory<CelestialObjectCatalogDbContext>
    {
        public CelestialObjectCatalogDbContext CreateDbContext(string[] args)
        {
            Console.WriteLine("ana are mere!");
            var optionsBuilder = new DbContextOptionsBuilder<CelestialObjectCatalogDbContext>();

            optionsBuilder.UseSqlServer("Data Source=DESKTOP-VQ4KD11;Initial Catalog=CelestialObjectCatalog;Integrated Security=True");

            return new CelestialObjectCatalogDbContext(optionsBuilder.Options);
        }
    }


    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder
                .Register( _ =>
                {
                    //create the builder
                    var builder = new DbContextOptionsBuilder<CelestialObjectCatalogDbContext>();

                    //use sql server
                    builder.UseSqlServer(
                        "Data Source=DESKTOP-VQ4KD11;Initial Catalog=CelestialObjectCatalog;Integrated Security=True");

                    //get options
                    return builder.Options;
                })
                .SingleInstance();


            containerBuilder
                .RegisterType<CelestialObjectCatalogDbContext>()
                .AsSelf()
                .InstancePerDependency();

            var services = containerBuilder.Build();


            //resolve db context
            var dbContext = services.Resolve<CelestialObjectCatalogDbContext>();

            var repo = new AbstractRepository<DiscoverySource, Guid>(dbContext);

            //await repo.AddAsync(new DiscoverySource
            //{
            //    EstablishmentDate = DateTime.Now,
            //    Name = "eduardo",
            //    StateOwner = "USa",
            //    Type = DiscoverySourceType.GroundTelescope
            //});

            var a = await repo.GetAllAsync();

            var r = await a.FirstAsync(x => x.Name == "eduardo");


        }
    }
}
