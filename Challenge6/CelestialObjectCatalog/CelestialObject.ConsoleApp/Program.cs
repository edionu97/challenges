using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Autofac;
using CelestialObjectCatalog.Classifier.Engine;
using CelestialObjectCatalog.Classifier.Engine.Impl;
using CelestialObjectCatalog.Classifier.Rules;
using CelestialObjectCatalog.Classifier.Rules.Impl;
using CelestialObjectCatalog.Persistence.Context;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Repository;
using CelestialObjectCatalog.Persistence.Repository.Impl;
using CelestialObjectCatalog.Persistence.UnitOfWork;
using CelestialObjectCatalog.Persistence.UnitOfWork.Impl;
using CelestialObjectCatalog.Services.Celestial;
using CelestialObjectCatalog.Services.Celestial.Impl;
using CelestialObjectCatalog.Services.Source;
using CelestialObjectCatalog.Services.Source.Impl;
using CelestialObjectCatalog.Services.Statistics;
using CelestialObjectCatalog.Services.Statistics.Impl;
using Deveel.Math;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CelestialObject = CelestialObjectCatalog.Persistence.Models.CelestialObject;

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
                .Register(_ =>
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

            containerBuilder
                .RegisterType<CatalogStatisticsService>()
                .As<IStatisticsService>()
                .SingleInstance();

            containerBuilder
                .RegisterType<StarClassificationRule>()
                .As<ICelestialObjectClassificationRule>();

            containerBuilder
                .RegisterType<PlanetClassificationRule>()
                .As<ICelestialObjectClassificationRule>();

            containerBuilder
                .RegisterType<BlackHoleClassificationRule>()
                .As<ICelestialObjectClassificationRule>();

            containerBuilder
                .RegisterType<CelestialObjectClassifier>()
                .As<ICelestialObjectClassifier>();


            var container = containerBuilder.Build();

            var statisticsService = container.Resolve<IStatisticsService>();

            var celestialObjectService = container.Resolve<ICelestialObjectService>();

            var discoverySourceService = container.Resolve<IDiscoverySourceService>();

            var classifier = container.Resolve<ICelestialObjectClassifier>();

            var aa = BigDecimal.Parse("345e540000");

            var item = BigDecimal.Parse("1");

            var b = BigMath.Divide(item, aa, MathContext.Decimal128);


        }

        private static Expression<Func<DiscoverySource, T>> S<T>(Expression<Func<DiscoverySource, T>> x)
        {
            return x;
        }
    }
}
