using System;
using Autofac;
using CelestialObjectCatalog.Persistence.Context;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Repository;
using CelestialObjectCatalog.Persistence.Repository.Impl;
using CelestialObjectCatalog.Persistence.UnitOfWork;
using CelestialObjectCatalog.Persistence.UnitOfWork.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CelestialObjectCatalog.WebApi.IoC.Modules
{
    /// <summary>
    /// This module contains all instances that are related to persistence layer
    /// </summary>
    public class PersistenceModule : Module
    {
        /// <summary>
        /// Define all the components related to persistence layer
        /// </summary>
        /// <param name="builder">The container builder</param>
        protected override void Load(ContainerBuilder builder)
        {
            //register db context options as singleton
            builder
                .Register(c =>
                {
                    var config = c.Resolve<IConfiguration>();

                    //create the builder
                    var dbContextOptionsBuilder =
                        new DbContextOptionsBuilder<CelestialObjectCatalogDbContext>();

                    //use sql server
                    dbContextOptionsBuilder
                        .UseSqlServer(config
                            .GetConnectionString("DefaultConnection"));

                    //get options
                    return dbContextOptionsBuilder.Options;
                })
                .SingleInstance();

            //register db context once per http req
            builder
                .RegisterType<CelestialObjectCatalogDbContext>()
                .As<DbContext>()
                .InstancePerLifetimeScope();

            //register repositories in transient scope
            builder
                .RegisterType<DiscoverySourceRepository>()
                .As<IRepository<DiscoverySource, Guid>>()
                .InstancePerDependency();

            builder
                .RegisterType<CelestialObjectDiscoveryRepository>()
                .As<IRepository<CelestialObjectDiscovery, Guid>>()
                .InstancePerDependency();

            builder
                .RegisterType<CelestialObjectRepository>()
                .As<IRepository<CelestialObject, Guid>>()
                .InstancePerDependency();

            //register the unit of work as single instance per http req
            builder
                .RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();
        }
    }
}
