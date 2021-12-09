using Autofac;
using CelestialObjectCatalog.Services.Celestial;
using CelestialObjectCatalog.Services.Celestial.Impl;
using CelestialObjectCatalog.Services.Source;
using CelestialObjectCatalog.Services.Source.Impl;
using CelestialObjectCatalog.Services.Statistics;
using CelestialObjectCatalog.Services.Statistics.Impl;

namespace CelestialObjectCatalog.WebApi.IoC.Modules
{
    /// <summary>
    /// This module contains all instances that represents a service class
    /// </summary>
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //register services as one instance per http req
            builder
                .RegisterType<DiscoverySourceService>()
                .As<IDiscoverySourceService>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<CelestialObjectService>()
                .As<ICelestialObjectService>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<CatalogStatisticsService>()
                .As<IStatisticsService>()
                .InstancePerLifetimeScope();
        }
    }
}
