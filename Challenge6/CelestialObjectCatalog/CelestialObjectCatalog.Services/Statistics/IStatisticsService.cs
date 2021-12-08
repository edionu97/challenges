using System.Collections.Generic;
using System.Threading.Tasks;
using CelestialObjectCatalog.Persistence.Models.Enums;

namespace CelestialObjectCatalog.Services.Statistics
{
    public interface IStatisticsService
    {
        /// <summary>
        /// This method should be used for getting all discovery sources which have discovered the maximum number of
        /// celestial objects of a given type
        /// </summary>
        /// <param name="objectType">The object type</param>
        /// <returns>A list of discovery source and its total number of object discovery</returns>
        public Task<IEnumerable<(string CountryName, int ObjectsDiscovered)>>
            FindDiscoverySourcesWithMostObjectTypeDiscoveriesAsync(CelestialObjectType objectType);
    }
}
