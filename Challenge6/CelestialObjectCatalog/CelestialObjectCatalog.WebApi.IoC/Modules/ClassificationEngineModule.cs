using Autofac;
using CelestialObjectCatalog.Classifier.Engine;
using CelestialObjectCatalog.Classifier.Engine.Impl;
using CelestialObjectCatalog.Classifier.Rules;
using CelestialObjectCatalog.Classifier.Rules.Impl;

namespace CelestialObjectCatalog.WebApi.IoC.Modules
{
    /// <summary>
    /// This module contains all components related to classification rules
    /// </summary>
    public class ClassificationEngineModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //register all rules as singleton
            builder
                .RegisterType<StarClassificationRule>()
                .As<ICelestialObjectClassificationRule>()
                .SingleInstance();

            builder
                .RegisterType<PlanetClassificationRule>()
                .As<ICelestialObjectClassificationRule>()
                .SingleInstance();

            builder
                .RegisterType<BlackHoleClassificationRule>()
                .As<ICelestialObjectClassificationRule>()
                .SingleInstance();

            //register the classification engine as singleton
            builder
                .RegisterType<CelestialObjectClassificationEngine>()
                .As<ICelestialObjectClassificationEngine>()
                .SingleInstance();
        }
    }
}
