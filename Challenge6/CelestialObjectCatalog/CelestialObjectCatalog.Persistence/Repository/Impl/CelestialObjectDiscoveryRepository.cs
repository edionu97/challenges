using System;
using Microsoft.EntityFrameworkCore;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Repository.Impl.Abstract;

namespace CelestialObjectCatalog.Persistence.Repository.Impl
{
    public class CelestialObjectDiscoveryRepository : AbstractRepository<CelestialObjectDiscovery, Guid>
    {
        public CelestialObjectDiscoveryRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
