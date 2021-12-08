using System;
using System.Linq;
using System.Linq.Expressions;
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
using CelestialObjectCatalog.Services.Celestial;
using CelestialObjectCatalog.Services.Celestial.Impl;
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

            Expression<Func<DiscoverySource, object>> x = y => y.Name;

            var a = x.ToString();
            
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
                .As<IRepository<DiscoverySource, Guid>>()
                .InstancePerDependency();

            containerBuilder
                .RegisterType<CelestialObjectDiscoveryRepository>()
                .As<IRepository<CelestialObjectDiscovery, Guid>>()
                .InstancePerDependency();

            containerBuilder
                .RegisterType<CelestialObjectRepository>()
                .As<IRepository<CelestialObjectCatalog.Persistence.Models.CelestialObject, Guid>>()
                .InstancePerDependency();

            containerBuilder
                .RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .SingleInstance();//replace with instance per request

            containerBuilder
                .RegisterType<DiscoverySourceService>()
                .As<IDiscoverySourceService>()
                .SingleInstance();

            containerBuilder
                .RegisterType<CelestialObjectService>()
                .As<ICelestialObjectService>()
                .SingleInstance();


            var container = containerBuilder.Build();

            var service = container.Resolve<ICelestialObjectService>();

            //await service.AddAsync(
            //    "aurora 2",
            //    2.5e24,
            //    24410000,
            //    9800,
            //    "Hubble"
            //);


            var l = await service.GetObjectsDiscoveredByCountryAsync("USA");


        }

        private static Expression<Func<DiscoverySource, T>> S<T>(Expression<Func<DiscoverySource, T>> x)
        {
            return x;
        }
    }
}
