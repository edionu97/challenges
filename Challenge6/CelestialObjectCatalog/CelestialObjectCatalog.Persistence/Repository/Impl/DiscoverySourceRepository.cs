using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Repository.Impl.Abstract;
using CelestialObjectCatalog.Utility.Items;

namespace CelestialObjectCatalog.Persistence.Repository.Impl
{
    public class DiscoverySourceRepository : AbstractRepository<DiscoverySource, Guid>
    {
        public DiscoverySourceRepository(DbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
