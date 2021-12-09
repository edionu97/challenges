using Autofac;
using CelestialObjectCatalog.WebApi.IoC.Modules;

namespace CelestialObjectCatalog.WebApi.IoC
{
    public static class WebApiBootstrapper
    {
        /// <summary>
        /// Assemble all components, using DI modules for better separation of concerns in DI
        /// </summary>
        /// <param name="containerBuilder">The container builder</param>
        public static void Bootstrap(ContainerBuilder containerBuilder)
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
    }
}
