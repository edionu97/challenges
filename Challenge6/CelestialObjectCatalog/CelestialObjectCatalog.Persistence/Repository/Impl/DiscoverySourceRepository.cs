using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Repository.Impl.Abstract;
using CelestialObjectCatalog.Utility.Items;

namespace CelestialObjectCatalog.Persistence.Repository.Impl
{
    public class DiscoverySourceRepository : AbstractRepository<DiscoverySource, Guid>, IDiscoverySourceRepository
    {
        public DiscoverySourceRepository(DbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<Maybe<DiscoverySource>> 
            GetDiscoverySourceByNameAsync(string discoverySourceName)
        {
            //get all objects with desired name
            var objects =
                (await FindAsync(x => x.Name == discoverySourceName)).ToList();

            //if there is any object return maybe of object otherwise return empty maybe
            return objects.Any()
                ? Maybe.Some(objects.First())
                : Maybe.None<DiscoverySource>();
        }
    }
}
