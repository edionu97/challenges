using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Autofac;
using CelestialObjectCatalog.Persistence.Context;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Models.Enums;
using CelestialObjectCatalog.Persistence.Repository;
using CelestialObjectCatalog.Persistence.Repository.Impl;
using CelestialObjectCatalog.Persistence.Repository.Impl.Abstract;
using CelestialObjectCatalog.Persistence.UnitOfWork;
using CelestialObjectCatalog.Persistence.UnitOfWork.Impl;
using CelestialObjectCatalog.Services.Source;
using CelestialObjectCatalog.Services.Source.Impl;
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
                .As<DbContext>()
                .SingleInstance();//replace with instance per request

            containerBuilder
                .RegisterType<DiscoverySourceRepository>()
                .As<IDiscoverySourceRepository>()
                .InstancePerDependency();

            containerBuilder
                .RegisterType<CelestialObjectDiscoveryRepository>()
                .As<IRepository<CelestialObjectDiscovery, Guid>>()
                .InstancePerDependency();

            containerBuilder
                .RegisterType<CelestialObjectRepository>()
                .As<ICelestialObjectRepository>()
                .InstancePerDependency();

            containerBuilder
                .RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .SingleInstance();//replace with instance per request

            containerBuilder
                .RegisterType<DiscoverySourceService>()
                .As<IDiscoverySourceService>()
                .SingleInstance();


            var container = containerBuilder.Build();

            var service = container.Resolve<IDiscoverySourceService>();

            var i = await service.FindDiscoverySourceByPartialName("e");

            foreach (var item in await service.GetAllAsync())
            {
                Console.WriteLine(item.Name + " " + item.Type);
            }

        }
    }
}
