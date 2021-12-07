using System;
using System.Threading.Tasks;
using CelestialObjectCatalog.Utility.Items;
using CelestialObjectCatalog.Persistence.Models;

namespace CelestialObjectCatalog.Persistence.Repository
{
    public interface IDiscoverySourceRepository : IRepository<DiscoverySource, Guid>
    {
        /// <summary>
        /// This function should be used for getting a discovery source by its name
        /// </summary>
        /// <param name="discoverySourceName">Name of the discovery source</param>
        /// <returns>A maybe containing the element or an empty maybe</returns>
        public Task<Maybe<DiscoverySource>> 
            GetDiscoverySourceByNameAsync(string discoverySourceName);
    }
}
