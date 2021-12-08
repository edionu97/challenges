using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CelestialObjectCatalog.Persistence.UnitOfWork;
using CelestialObjectCatalog.Persistence.Models.Enums;

namespace CelestialObjectCatalog.Services.Statistics.Impl
{

    public class CatalogStatisticsService : IStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CatalogStatisticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<(string CountryName, int ObjectsDiscovered)>> 
            FindDiscoverySourcesWithMostObjectTypeDiscoveriesAsync(CelestialObjectType objectType)
        {
            //get all discoveries that are targeting desired object type 
            var allObjectTypeDiscoveries = 
                await _unitOfWork
                    .CelestialDiscoveriesRepo
                    .GetAllAsQueryable()
                    .Include(c => c.CelestialObject)
                    .Include(c => c.DiscoverySource)
                    .Where(c => c.CelestialObject.Type == objectType)
                    .ToListAsync();

            //if there are no discoveries return an empty list
            if (!allObjectTypeDiscoveries.Any())
            {
                return Enumerable.Empty<(string CountryName, int ObjectsDiscovered)>();
            }

            //get no of maximum discovery
            var maximumDiscoveries = allObjectTypeDiscoveries
                .GroupBy(x => x.DiscoverySource.StateOwner)
                .Max(x => x.Count());

            //return all countries with maximum discovery
            return allObjectTypeDiscoveries
                .GroupBy(x => x.DiscoverySource.StateOwner)
                .Where(group => group.Count() == maximumDiscoveries)
                .Select(x => (x.Key, maximumDiscoveries));
        }
    }
}
